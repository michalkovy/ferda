SectionGroup "$(Boxes)" Boxes
	
	Section "$(DataPreparation)" DataPreparation
	SetOutPath "$INSTDIR\Server\BoxModulesServices\DataPreparation"
	File /r ..\bin\Server\BoxModulesServices\DataPreparation\*.*
	SectionEnd
	
	Section "$(GuhaMining)" GuhaMining
	SetOutPath "$INSTDIR\Server\BoxModulesServices\GuhaMining"
	File /r ..\bin\Server\BoxModulesServices\GuhaMining\*.*
	SectionEnd
	
	Section "$(Sample)" Sample
	SetOutPath "$INSTDIR\Server\BoxModulesServices\Sample"
	File /r ..\bin\Server\BoxModulesServices\Sample\*.*
	SectionEnd
	
SectionGroupEnd