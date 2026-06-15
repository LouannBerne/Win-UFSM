# Win-USFM (User Storage File Manager)

Application Windows Forms pour analyser l’espace disque occupé par les dossiers utilisateurs de `C:\Users`.

## Fonctionnalités

- Liste les dossiers utilisateurs (hors dossiers système courants)
- Calcule la taille de chaque dossier
- Affiche le poids total
- Permet de changer l’unité d’affichage (Auto, To, Go, Mo, Ko, Octets)
- Permet de supprimer les dossiers cochés (avec confirmation)

## Prérequis

- Windows
- .NET SDK 10 (target `net10.0-windows`)

## Exécution

Depuis la racine du dépôt :

```bash
dotnet run --project stage-appStorage/stage-appStorage.csproj
```

## Build

```bash
dotnet build stage-appStorage/stage-appStorage.csproj
```

## Attention

- L’application lit et supprime des dossiers dans `C:\Users`.
- La suppression est définitive.
- Exécuter avec prudence et uniquement sur une machine que vous contrôlez.
