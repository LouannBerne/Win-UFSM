using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stage_appStorage
{
    public partial class FormApp : Form
    {
        public FormApp()
        {
            InitializeComponent();
        }

        private async void FormApp_Load(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                lstUsers.Items.Clear();
                btnRefresh.Enabled = false;
                btnDelete.Enabled = false;
                cmbUnit.Enabled = false;

                progressBar.Value = 0;
                progressBar.Visible = true;

                string usersPath = @"C:\Users";
                long totalSize = 0;

                if (Directory.Exists(usersPath))
                {
                    var dirs = Directory.GetDirectories(usersPath);

                    // Exclure les dossiers systèmes connus comme "All Users" et "Default"
                    var excludedFolders = new[] { "All Users", "Default", "Default User", "Public" };

                    var validDirs = dirs.Where(d => !excludedFolders.Contains(Path.GetFileName(d), StringComparer.OrdinalIgnoreCase)).ToList();

                    if (validDirs.Count > 0)
                    {
                        progressBar.Maximum = validDirs.Count;
                    }

                    foreach (var dir in validDirs)
                    {
                        string folderName = Path.GetFileName(dir);

                        long size = await Task.Run(() => GetDirectorySize(new DirectoryInfo(dir)));
                        totalSize += size;

                        string[] row = { folderName, FormatBytes(size) };
                        var item = new ListViewItem(row);

                        // Sauvegarde du chemin complet (Item1) et de la taille brute (Item2) dans la propriété Tag
                        item.Tag = new Tuple<string, long>(dir, size); 

                        lstUsers.Items.Add(item);

                        if (progressBar.Value < progressBar.Maximum)
                            progressBar.Value++;
                    }
                }

                // Mettre à jour et stocker en Tag le totalSize global pour qu'il s'actualise avec les unités
                lblTotalSize.Tag = totalSize;
                lblTotalSize.Text = $"Poids total : {FormatBytes(totalSize)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement : " + ex.Message);
            }
            finally
            {
                progressBar.Visible = false;
                btnRefresh.Enabled = true;
                btnDelete.Enabled = true;
                cmbUnit.Enabled = true;
            }
        }

        private long GetDirectorySize(DirectoryInfo d)
        {
            long size = 0;
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += GetDirectorySize(di);
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (Exception) { }
            return size;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var selectedItems = lstUsers.CheckedItems;
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins un dossier à supprimer.");
                return;
            }

            var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {selectedItems.Count} dossier(s) ainsi que les clés de registre associées ? Attention, cette action est définitive.", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                foreach (ListViewItem item in selectedItems)
                {
                    var tagData = item.Tag as Tuple<string, long>;
                    if (tagData == null) continue;

                    string path = tagData.Item1;
                    try
                    {
                        if (!DeleteProfileViaWMI(path))
                        {
                            // Fallback if WMI fails or does not find the profile
                            RemoveReadonlyAttributes(path);
                            Directory.Delete(path, true);
                            DeleteUserRegistryProfile(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la suppression de {path}: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                _ = LoadUsersAsync();
            }
        }

        private bool DeleteProfileViaWMI(string profilePath)
        {
            try
            {
                var query = new SelectQuery("Win32_UserProfile", $"LocalPath='{profilePath.Replace("\\", "\\\\")}'");
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject profile in searcher.Get())
                    {
                        profile.Delete();
                        return true; // The profile folder and registry key were successfully deleted via WMI
                    }
                }
            }
            catch (Exception)
            {
                // Silent catch, let fallback handle it
            }
            return false;
        }

        private void RemoveReadonlyAttributes(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath)) return;

                var dirInfo = new DirectoryInfo(directoryPath);

                // Normaliser les attributs de tous les fichiers
                foreach (var file in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    file.Attributes = FileAttributes.Normal;
                }

                // Normaliser les attributs des sous-dossiers
                foreach (var dir in dirInfo.EnumerateDirectories("*", SearchOption.AllDirectories))
                {
                    dir.Attributes = FileAttributes.Normal;
                }
            }
            catch (Exception)
            {
                // Ignore errors related to access denied during attribute reset, 
                // the subsequent Directory.Delete might still throw.
            }
        }

        private void DeleteUserRegistryProfile(string profilePath)
        {
            try
            {
                string profileListPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
                using (var localMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64))
                {
                    using (var profileListKey = localMachine.OpenSubKey(profileListPath, true))
                    {
                        if (profileListKey != null)
                        {
                            foreach (string subKeyName in profileListKey.GetSubKeyNames())
                            {
                                using (var subKey = profileListKey.OpenSubKey(subKeyName))
                                {
                                    if (subKey != null)
                                    {
                                        var pathValue = subKey.GetValue("ProfileImagePath") as string;
                                        if (pathValue != null && pathValue.Equals(profilePath, StringComparison.OrdinalIgnoreCase))
                                        {
                                            profileListKey.DeleteSubKeyTree(subKeyName);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression de la clé de registre pour le profil {profilePath}: {ex.Message}", "Erreur Registre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstUsers.Items)
            {
                var tagData = item.Tag as Tuple<string, long>;
                if (tagData != null)
                {
                    long originalSize = tagData.Item2;
                    item.SubItems[1].Text = FormatBytes(originalSize);
                }
            }

            if (lblTotalSize.Tag != null && lblTotalSize.Tag is long totalSize)
            {
                lblTotalSize.Text = $"Poids total : {FormatBytes(totalSize)}";
            }
        }

        private string FormatBytes(long bytes)
        {
            string unit = cmbUnit.SelectedItem?.ToString() ?? "Auto";

            if (unit == "Auto")
            {
                string[] suffixes = { "Octets", "Ko", "Mo", "Go", "To", "Po" };
                int counter = 0;
                decimal number = (decimal)bytes;
                while (Math.Round(number / 1024) >= 1)
                {
                    number /= 1024;
                    counter++;
                }
                return string.Format("{0:n2} {1}", number, suffixes[counter]);
            }
            else
            {
                decimal number = (decimal)bytes;
                string suffix = unit;

                switch (unit)
                {
                    case "Ko": number /= 1024; break;
                    case "Mo": number /= (1024 * 1024); break;
                    case "Go": number /= (1024 * 1024 * 1024); break;
                    case "To": number /= (1024M * 1024M * 1024M * 1024M); break;
                    case "Octets": break; // inchangé
                }

                return string.Format("{0:n2} {1}", number, suffix);
            }
        }
    }
}
