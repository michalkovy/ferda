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
	
	Section "$(OntologyRelated)" OntologyRelated
	SetOutPath "$INSTDIR\Server\BoxModulesServices\OntologyRelated"
	File /r ..\bin\Server\BoxModulesServices\OntologyRelated\*.*
	SectionEnd
	
	Section "$(Language)" Language
	SetOutPath "$INSTDIR\Server\BoxModulesServices\Language"
	File /r ..\bin\Server\BoxModulesServices\Language\*.*
	SectionEnd	
	
	Section "$(Wizards)" Wizards
	SetOutPath "$INSTDIR\Server\BoxModulesServices\Wizards"
	File /r ..\bin\Server\BoxModulesServices\Wizards\*.*
	SectionEnd

	Section "$(SemanticWeb)" SemanticWeb
	SetOutPath "$INSTDIR\Server\BoxModulesServices\SemanticWeb"
	File /r ..\bin\Server\BoxModulesServices\SemanticWeb\*.*
	SectionEnd
	
SectionGroupEnd
