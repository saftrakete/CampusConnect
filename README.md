# CampusConnect

Dies ist das Repository für das Softwareprojekt.

### Installations-Anleitung:

- fork installieren 
- node js installieren

in die .sln Datei in Visual Studio Code öffnen
im terminal: <br/>
	- npm install -g @angular/cli  <br/>
ein test Befehl, ob korrekt installiert: <br/>
	- ng g c  \n <br/>


### Hinweis zum Erstellen der Datenbank:
 1. Projekt in Visual Studio öffnen
 2. Paket-Manager-Konsole aufrufen
      -  sollte diese nicht bereits unten mit angezeigt werden, dann
         oben in der Leiste auf "Ansicht" -> "Weitere Fenster" -> "Paket-Manager-Konsole"
 3. Eingeben: add-migration "name" (z.B. add-migration init)
 4. Danach eingeben: update-database
 5. Die Datenbank sollte erstellt werden, über "Ansicht" kann der SQL-Server-Objekt-Explorer geöffnet werden

### Hinweis zum Hinzufügen von Services im Backend:
	> Wir nutzen Dependency Injection, um die Services unseren Controllern etc. bereitzustellen
 	> Damit das funktioniert, müssen die Services in Program.cs registriert werden (in der ServiceCollection)
  	> Good Practice wäre hierbei, für jeden Service ein Interface anzulegen
   	> mögliche Befehle zum Registrieren:
    		1. builder.Services.AddTransient<IService, Service>(); <- es wird immer eine neue Instanz des Services erzeugt <br/>
      		2. builder.Services.AddSingleton<IService, Service>(); <- es existiert eine einzige Instanz des Services (Singleton) <br/>

### Dateien: <br/>
[Code-Richtlinien.pdf](https://github.com/user-attachments/files/19986317/Code-Richtlinien.pdf) <br/>
[Branches, Commits, Pull Requests.pdf](https://github.com/user-attachments/files/19986316/Branches.Commits.Pull.Requests.pdf)
