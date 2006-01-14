SectionGroup "$(Boxes)" Boxes
	
	Section "FFTTask" FFTTask
	SetOutPath "$INSTDIR\Server\BoxModulesServices\LISpMinerTasks\FFTTask"
	File /r ..\bin\Server\BoxModulesServices\LISpMinerTasks\FFTTask\*.*
	SectionEnd
	
	Section "CFTask" CFTask
	SetOutPath "$INSTDIR\Server\BoxModulesServices\LISpMinerTasks\CFTask"
	File /r ..\bin\Server\BoxModulesServices\LISpMinerTasks\CFTask\*.*
	SectionEnd
	
	Section "KLTask" KLTask
	SetOutPath "$INSTDIR\Server\BoxModulesServices\LISpMinerTasks\KLTask"
	File /r ..\bin\Server\BoxModulesServices\LISpMinerTasks\KLTask\*.*
	SectionEnd
	
	Section "SDFFTTask" SDFFTTask
	SetOutPath "$INSTDIR\Server\BoxModulesServices\LISpMinerTasks\SDFFTTask"
	File /r ..\bin\Server\BoxModulesServices\LISpMinerTasks\SDFFTTask\*.*
	SectionEnd
	
	Section "SDKLTask" SDKLTask
	SetOutPath "$INSTDIR\Server\BoxModulesServices\LISpMinerTasks\SDKLTask"
	File /r ..\bin\Server\BoxModulesServices\LISpMinerTasks\SDKLTask\*.*
	SectionEnd
	
	Section "SDCFTask" SDCFTask
	SetOutPath "$INSTDIR\Server\BoxModulesServices\LISpMinerTasks\SDCFTask"
	File /r ..\bin\Server\BoxModulesServices\LISpMinerTasks\SDCFTask\*.*
	SectionEnd
	
SectionGroupEnd