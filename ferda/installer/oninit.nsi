!define MUI_LANGDLL_WINDOWTITLE "$example"
!define MUI_LANGDLL_INFO "$example1"

Var example
Var example1
;for some reason, it works only through function...
Function testVar
  StrCpy $example "Installer Language"
  StrCpy $example1 "Please select a language:"
FunctionEnd

  
;What to do in the beginning
Function .onInit
   call testVar

	!insertmacro MUI_LANGDLL_DISPLAY
	
	Call PreviousFerdaInstalled
	Pop $0
	StrCmp $0 "" CheckDotNet FerdaInstalled 
	
	FerdaInstalled:
	MessageBox MB_YESNO "$(Old_Ferda)$0. $(Uninstall_Old_Ferda)" IDYES UninstallOldFerda
	goto CheckDotNet
	
	CheckDotNet:
  Call IsExactDotNetInstalled
   Pop $0
   StrCmp $0 1 bad.NETFramework GoodDotNet

   GoodDotNet:
  Call IsDotNETInstalledAdv
   Pop $0
   StrCmp $0 1 done no.NETFramework
  
   bad.NETFramework:
  MessageBox MB_YESNO "$(Bad_Dot_Net_Installed)" IDYES GoodDotNet 
  Abort "Exiting..."
   
   no.NETFramework:
  MessageBox MB_YESNO "$(Dot_Net_Not_Installed) ${DOT_MAJOR}.${DOT_MINOR}.${DOT_MINOR_MINOR}. $(Dot_Net_Not_Installed1)" IDYES NoDotNet
  Abort "Exiting..."
  ; asked to exit the install
  
  NoDotNet:
  Goto done
  
  UninstallOldFerda:
  Call GetFerdaPath
  Pop $0
  StrCmp $0 "" done +1
  Exec '"$0\Uninstall.exe"'
  
  done:
   
 FunctionEnd