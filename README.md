# ML-Agents
ML-Agents Unity Project for university

Projektname:
Unity Machine Learning Agents (ML-Agents)

Teammitglieder:
Mattis Lemmrich (202208), Dominik Urbani (202583)

Besonderheiten:
Das Projekt besteht aus mehreren Szenen. Die meisten davon haben wir zum Trainieren der Models benutzt. Um diese zu benutzen, muss ein virtual Environment im Projektordner vorhanden sein, in diesem ML-Agents installiert und im Lernmodus gestartet werden. Der Lernprozess muss je Scene mit einem entsprechenden config-File gestartet werden, so dass er mit den richtigen Parametern läuft. Da dies alles für die allgemeine Nutzung zu kompliziert wäre und man sich im Projekt sehr gut auskennen muss, haben wir eine Szene erstellt, die unseren Projektverlauf anhand von mehreren, aufeinander aufbauenden Sub-Leveln zeigt (Assets/Scenes/Präsentation/ShowScene). Dabei besitzen die unterschiedlichen Agents ihre final trainierten Models, so dass beim Start der Szene die untschiedlichen antrainierten Verhalten direkt gezeigt werden.
Besondere Leistungen, Herausforderungen und gesammelte Erfahrungen, Was hat die meiste Zeit gekostet?:
Besonders herausheben kann man, wie viel Zeit es uns gekostet hat die Models zu trainieren. Vor allem die Models für unsere großen Level mussten wir öfter Nächtelang trainieren um ein Ergebnis zu erzielen, welches dann oft doch nicht unseren Erwartungen entsprach. Da lag auch eine der größten Herausforderungen in unserem Projekt. Die Level für das Training aufzubauen und die richtigen Parameter für das trainieren der Models zu finden, so dass das Model genug Informationen hat um das Level korrekt zu spielen. Vor allem haben wir das im finalen Level festgestellt, da dort ein weiterer Action-Parameter (Möglichkeit eine Kiste zu tragen) dazu geführt hat, dass der Lernprozess komplett neu strukturiert werden musste. Diese Umstrukturierung war so aufwendig, dass wir erst wenige Tage vor der Abschlusspräsentation einen Durchbruch erreichten. Wir haben gelernt, was es für einen großen Unterschied in der Geschwindigkeit beim Trainieren der Models gibt, sobald man verschiedene Sensoren anwendet. Außerdem wurde uns klar, dass man ein gut erzieltes Ergebnis mit einem Sensor nicht erneut erzielen kann, wenn man nur den Sensor tauscht und den Rest des Environments beibehält. Hier hatten wir die große Herausforderung für jeden Sensor die perfekten Einstellungen und Parameter zu finden um ähnliche Ergebnisse zu erzielen. Bei den verschiedenen Sensoren machte uns vor allem der Kamera Sensor die größten Probleme. Zusätzlich haben wir es unterschätzt, wie wichtig es ist, dem neuronalen Netz genügend Observations zur Verfügung zu stellen, da dieses genauso wie ein Mensch Eindrücke des Levels benötigt um es richtig Spielen zu können. 


Verwendete Assets, Codefragmente, Inspiration.
Assets:
•	ML-Agents 2.1.0-exp.1 (preview)
•	TextMeshPro
Inspiration:
•	https://www.youtube.com/watch?v=ZKzXAVp8bC8: 
o	Tipps und Übernahme einer Grund .yaml-Datei die im späteren Projektverlauf selbstständig erweitert wurde 
•	https://www.immersivelimit.com/tutorials/ml-agents-camera-vision-coin-collector:
o	Tipps und Einführung beim Kamera-Sensor
o	Idee für kleines Beispiel-Level
•	https://www.youtube.com/watch?v=S0nciyOTh88:
o	Einführung in Curriculum Learning
•	https://www.youtube.com/watch?v=zPFU30tbyKs&t=445s:
o	Installation von ML-Agents und Übernahme des Basic-Levels für das Lernen der Grundlagen und die Präsentation (Assets/Scenes/Präsentation/SampleScene)
