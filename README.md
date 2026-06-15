# Win-USFM (User Storage File Manager)

[![Version](https://img.shields.io/badge/version-v1.5.0-blue.svg)](https://github.com/louannberne)
[![Target](https://img.shields.io/badge/.NET-10.0--windows-purple.svg)](https://dotnet.microsoft.com/fr/download/dotnet/10.0)

Application Windows Forms (.NET 10) conçue pour analyser l'espace disque occupé par les dossiers utilisateurs de `C:\Users` et permettre un nettoyage propre, complet et sécurisé de l'environnement.

---

## 🛠️ Fonctionnalités incluses

### 📊 Analyse & Visualisation de l'Espace Disque
* **Scrutine automatique :** Scan complet et asynchrone (`Task.Run`) du répertoire `C:\Users` pour lister les dossiers utilisateurs existants sans bloquer l'interface.
* **Filtres intelligents :** Exclusion automatique des dossiers systèmes et partagés connus (`All Users`, `Default`, `Default User`, `Public`).
* **Conversion d'unités à la volée :** Possibilité de modifier l'affichage des tailles en un clic (Auto, To, Go, Mo, Ko, Octets) avec recalcul instantané du poids total accumulé.

### 🧹 Nettoyage complet (Fichiers & Registre)
* **Suppression définitive :** Possibilité de sélectionner et de purger plusieurs dossiers utilisateurs simultanément (avec boîte de dialogue de confirmation de sécurité).
* **Nettoyage du Registre Windows :** Suppression propre et sécurisée de l'arborescence correspondante dans la clé de registre `ProfileList` (`SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList`) afin d'éviter les résidus de profils orphelins dans le système.

---

## 🔧 Améliorations & Robustesse technique

* **Calcul tolérant aux pannes :** Gestion asynchrone des exceptions d'accès refusé (`UnauthorizedAccessException`) lors du calcul de la taille des fichiers et sous-dossiers.
* **Exécution 64-bit :** Ouverture ciblée de la ruche `RegistryHive.LocalMachine` en vue `Registry64` pour garantir la suppression des clés sur les systèmes d'exploitation modernes.
* **Barre de progression :** Suivi visuel en temps réel de l'avancement de l'analyse des répertoires.

---

## 🚀 Démarrage & Utilisation

### 📦 Utilisation directe (Exécutable)
L'application est fournie prête à l'emploi sans obligation de compiler le code source :
1. Rendez-vous dans le dossier de build ou téléchargez la dernière version depuis l'onglet **Releases**.
2. Lancez directement le fichier exécutable **`Win_USFM-x64.exe`** situé dans le répertoire `Release`.

### 💻 Développement & Modification (Visual Studio)
Si vous souhaitez ouvrir, explorer ou modifier le projet :
* **Prérequis :** Disposer de [Visual Studio 2022](https://visualstudio.microsoft.com/fr/) (ou ultérieur) avec la charge de travail **Développement de bureau .NET** ainsi que le SDK **.NET 10.0**.
1. Ouvrez le fichier de solution (`.sln`) ou importez le projet `stage-appStorage.csproj` dans Visual Studio.
2. Pour générer une nouvelle version, sélectionnez la configuration `Release` dans la barre d'outils de Visual Studio et lancez la génération de la solution.

---

## ⚠️ Rappel de sécurité

> [!CAUTION]
> - L'application manipule et supprime définitivement des données de profils dans `C:\Users`. La suppression est définitive et irréversible.
> - Pour le bon fonctionnement du nettoyage des clés de registre associées aux profils Windows, l'application **doit impérativement être exécutée avec des privilèges d'administrateur** (Clic droit -> *Exécuter en tant qu'administrateur*).
> - Utilisez cet outil avec prudence et de préférence dans un environnement de test ou de déploiement maîtrisé.

---

## 👤 Auteur

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/louannberne">
        <img src="https://github.com/louannberne.png" width="100px;" alt="Louann Berne Profile Picture"/><br />
        <sub><b>Louann Berne</b></sub>
      </a>
    </td>
  </tr>
</table>
