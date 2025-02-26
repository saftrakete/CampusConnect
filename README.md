# CampusConnect

Dies ist das Repository für das Softwareprojekt.

Installations-Anleitung:

fork installieren
node js installieren

in die .sln Datei in Visual Studio Code öffnen
im terminal:
	> npm install -g @angular/cli <
ein test Befehl, ob korrekt installiert:
	> ng g c <

Visual Studio
in packet manager Konsole
	> add-migration init <
	> update-database <


Hinweis zum Erstellen der Datenbank:
 1. Projekt in Visual Studio öffnen
 2. Paket-Manager-Konsole aufrufen
      -  sollte diese nicht bereits unten mit angezeigt werden, dann
         oben in der Leiste auf "Ansicht" -> "Weitere Fenster" -> "Paket-Manager-Konsole"
 3. Eingeben: add-migration "name" (z.B. add-migration init)
 4. Danach eingeben: update-database
 5. Die Datenbank sollte erstellt werden, über "Ansicht" kann der SQL-Server-Objekt-Explorer geöffnet werden



