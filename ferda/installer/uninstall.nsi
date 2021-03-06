Section "Uninstall"

;!include "uninstall_functions.nsi"

;restore original .xfp association
!define Index "Line${__LINE__}"
  ReadRegStr $1 HKCR ".opt" ""
  StrCmp $1 "OptionsFile" 0 "${Index}-NoOwn" ; only do this if we own it
    ReadRegStr $1 HKCR ".opt" "backup_val"
    StrCmp $1 "" 0 "${Index}-Restore" ; if backup="" then delete the whole key
      DeleteRegKey HKCR ".opt"
    Goto "${Index}-NoOwn"
"${Index}-Restore:"
      WriteRegStr HKCR ".opt" "" $1
      DeleteRegValue HKCR ".opt" "backup_val"
   
    DeleteRegKey HKCR "OptionsFile" ;Delete key with association settings
 
"${Index}-NoOwn:"
!undef Index

  ;removing ice from path
  ReadRegStr $R2 HKCU "Software\Ferda DataMiner\"  "IcePath"
  Push "PATH"
  Push $R2
  Call un.RemoveFromEnvVar
  
  ;removing dlls from GAC
  Call un.DeleteFromGac
 
  Delete "$SMPROGRAMS\Ferda DataMiner\Uninstall.lnk"
  Delete "$SMPROGRAMS\Ferda DataMiner\Ferda DataMiner.lnk"
  RMDir "$SMPROGRAMS\Ferda DataMiner"
  Delete "$DESKTOP\Ferda DataMiner.lnk"
  Delete "$INSTDIR\Uninstall.exe"
  RMDir /r "$INSTDIR"
  
  ;cleaning registry
  DeleteRegKey HKCU "Software\Ferda DataMiner"

SectionEnd