!ifndef _AddToPath_nsh
!define _AddToPath_nsh
 
!verbose 3
!include "WinMessages.NSH"
!verbose 4
 
!ifndef WriteEnvStr_RegKey
  !ifdef ALL_USERS
    !define WriteEnvStr_RegKey \
       'HKLM "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"'
  !else
    !define WriteEnvStr_RegKey 'HKCU "Environment"'
  !endif
!endif
 
; AddToPath - Adds the given dir to the search path.
;        Input - head of the stack
;        Note - Win9x systems requires reboot
 /*
Function AddToPath
  Exch $0
  Push $1
  Push $2
  Push $3
 
  # don't add if the path doesn't exist
  
  MessageBox MB_OK "Path is $0"
  
  IfFileExists "$0\*.*" "" AddToPath_done
  
  
 
  ReadEnvStr $1 PATH
  Push "$1;"
  Push "$0;"
  
  
  
  Call StrStr
  Pop $2
  StrCmp $2 "" "" AddToPath_done
  MessageBox MB_OK "Going after"
  Push "$1;"
  Push "$0\;"
  Call StrStr
  Pop $2
  StrCmp $2 "" "" AddToPath_done
  GetFullPathName /SHORT $3 $0
  Push "$1;"
  Push "$3;"
  Call StrStr
  Pop $2
  StrCmp $2 "" "" AddToPath_done
  Push "$1;"
  Push "$3\;"
  Call StrStr
  Pop $2
  StrCmp $2 "" "" AddToPath_done
 
  Call IsNT
  Pop $1
  StrCmp $1 1 AddToPath_NT
    ; Not on NT
    StrCpy $1 $WINDIR 2
    FileOpen $1 "$1\autoexec.bat" a
    FileSeek $1 -1 END
    FileReadByte $1 $2
    IntCmp $2 26 0 +2 +2 # DOS EOF
      FileSeek $1 -1 END # write over EOF
    FileWrite $1 "$\r$\nSET PATH=%PATH%;$3$\r$\n"
    FileClose $1
    SetRebootFlag true
    Goto AddToPath_done
 
  AddToPath_NT:
    ReadRegStr $1 ${WriteEnvStr_RegKey} "PATH"
    StrCpy $2 $1 1 -1 # copy last char
    StrCmp $2 ";" 0 +2 # if last char == ;
      StrCpy $1 $1 -1 # remove last char
    StrCmp $1 "" AddToPath_NTdoIt
      StrCpy $0 "$1;$0"
    AddToPath_NTdoIt:
      WriteRegExpandStr ${WriteEnvStr_RegKey} "PATH" $0
      SendMessage ${HWND_BROADCAST} ${WM_WININICHANGE} 0 "STR:Environment" /TIMEOUT=5000
 
  AddToPath_done:
    Pop $3
    Pop $2
    Pop $1
    Pop $0
FunctionEnd
 
; RemoveFromPath - Remove a given dir from the path
;     Input: head of the stack
 
Function un.RemoveFromPath
  Exch $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $5
  Push $6
 
  IntFmt $6 "%c" 26 # DOS EOF
 
  Call un.IsNT
  Pop $1
  StrCmp $1 1 unRemoveFromPath_NT
    ; Not on NT
    StrCpy $1 $WINDIR 2
    FileOpen $1 "$1\autoexec.bat" r
    GetTempFileName $4
    FileOpen $2 $4 w
    GetFullPathName /SHORT $0 $0
    StrCpy $0 "SET PATH=%PATH%;$0"
    Goto unRemoveFromPath_dosLoop
 
    unRemoveFromPath_dosLoop:
      FileRead $1 $3
      StrCpy $5 $3 1 -1 # read last char
      StrCmp $5 $6 0 +2 # if DOS EOF
        StrCpy $3 $3 -1 # remove DOS EOF so we can compare
      StrCmp $3 "$0$\r$\n" unRemoveFromPath_dosLoopRemoveLine
      StrCmp $3 "$0$\n" unRemoveFromPath_dosLoopRemoveLine
      StrCmp $3 "$0" unRemoveFromPath_dosLoopRemoveLine
      StrCmp $3 "" unRemoveFromPath_dosLoopEnd
      FileWrite $2 $3
      Goto unRemoveFromPath_dosLoop
      unRemoveFromPath_dosLoopRemoveLine:
        SetRebootFlag true
        Goto unRemoveFromPath_dosLoop
 
    unRemoveFromPath_dosLoopEnd:
      FileClose $2
      FileClose $1
      StrCpy $1 $WINDIR 2
      Delete "$1\autoexec.bat"
      CopyFiles /SILENT $4 "$1\autoexec.bat"
      Delete $4
      Goto unRemoveFromPath_done
 
  unRemoveFromPath_NT:
    ReadRegStr $1 ${WriteEnvStr_RegKey} "PATH"
    StrCpy $5 $1 1 -1 # copy last char
    StrCmp $5 ";" +2 # if last char != ;
      StrCpy $1 "$1;" # append ;
    Push $1
    Push "$0;"
    Call un.StrStr ; Find `$0;` in $1
    Pop $2 ; pos of our dir
    StrCmp $2 "" unRemoveFromPath_done
      ; else, it is in path
      # $0 - path to add
      # $1 - path var
      StrLen $3 "$0;"
      StrLen $4 $2
      StrCpy $5 $1 -$4 # $5 is now the part before the path to remove
      StrCpy $6 $2 "" $3 # $6 is now the part after the path to remove
      StrCpy $3 $5$6
 
      StrCpy $5 $3 1 -1 # copy last char
      StrCmp $5 ";" 0 +2 # if last char == ;
        StrCpy $3 $3 -1 # remove last char
 
      WriteRegExpandStr ${WriteEnvStr_RegKey} "PATH" $3
      SendMessage ${HWND_BROADCAST} ${WM_WININICHANGE} 0 "STR:Environment" /TIMEOUT=5000
 
  unRemoveFromPath_done:
    Pop $6
    Pop $5
    Pop $4
    Pop $3
    Pop $2
    Pop $1
    Pop $0
FunctionEnd
 
 */
 
; AddToEnvVar - Adds the given value to the given environment var
;        Input - head of the stack $0 environement variable $1=value to add
;        Note - Win9x systems requires reboot
 
Function AddToEnvVar
 
  Exch $1 ; $1 has environment variable value
  Exch
  Exch $0 ; $0 has environment variable name
 
  DetailPrint "Adding $1 to $0"
  Push $2
  Push $3
  Push $4
 
 
  ReadEnvStr $2 $0
  Push "$2;"
  Push "$1;"
  Call StrStr
  Pop $3
;  StrCmp $3 "" "" AddToEnvVar_done
 
  Push "$2;"
  Push "$1\;"
  Call StrStr
  Pop $3
 ; StrCmp $3 "" "" AddToEnvVar_done
  
 
  Call IsNT
  Pop $2
  StrCmp $2 1 AddToEnvVar_NT
    ; Not on NT
    StrCpy $2 $WINDIR 2
    FileOpen $2 "$2\autoexec.bat" a
    FileSeek $2 -1 END
    FileReadByte $2 $3
    IntCmp $3 26 0 +2 +2 # DOS EOF
      FileSeek $2 -1 END # write over EOF
    FileWrite $2 "$\r$\nSET $0=%$0%;$4$\r$\n"
    FileClose $2
    SetRebootFlag true
    Goto AddToEnvVar_done
 
  AddToEnvVar_NT:
    ReadRegStr $2 ${WriteEnvStr_RegKey} $0
    StrCpy $3 $2 1 -1 # copy last char
    StrCmp $3 ";" 0 +2 # if last char == ;
      StrCpy $2 $2 -1 # remove last char
    StrCmp $2 "" AddToEnvVar_NTdoIt
      StrCpy $1 "$2;$1"
    AddToEnvVar_NTdoIt:
      WriteRegExpandStr ${WriteEnvStr_RegKey} $0 $1
      SendMessage ${HWND_BROADCAST} ${WM_WININICHANGE} 0 "STR:Environment" /TIMEOUT=5000
 
  AddToEnvVar_done:
    Pop $4
    Pop $3
    Pop $2
	Pop $1
    Pop $0
 
FunctionEnd
 
; RemoveFromEnvVar - Remove a given value from a environment var
;     Input: head of the stack
 
Function un.RemoveFromEnvVar
 
  Exch $1 ; $1 has environment variable value
  Exch
  Exch $0 ; $0 has environment variable name
 
  DetailPrint "Removing $1 from $0"
  Push $2
  Push $3
  Push $4
  Push $5
  Push $6
  Push $7
 
  IntFmt $7 "%c" 26 # DOS EOF
 
  Call un.IsNT
  Pop $2
  StrCmp $2 1 unRemoveFromEnvVar_NT
    ; Not on NT
    StrCpy $2 $WINDIR 2
    FileOpen $2 "$2\autoexec.bat" r
    GetTempFileName $5
    FileOpen $3 $5 w
    GetFullPathName /SHORT $1 $1
    StrCpy $1 "SET $0=%$0%;$1"
    Goto unRemoveFromEnvVar_dosLoop
 
    unRemoveFromEnvVar_dosLoop:
      FileRead $2 $4
      StrCpy $6 $4 1 -1 # read last char
      StrCmp $6 $7 0 +2 # if DOS EOF
        StrCpy $4 $4 -1 # remove DOS EOF so we can compare
      StrCmp $4 "$1$\r$\n" unRemoveFromEnvVar_dosLoopRemoveLine
      StrCmp $4 "$1$\n" unRemoveFromEnvVar_dosLoopRemoveLine
      StrCmp $4 "$1" unRemoveFromEnvVar_dosLoopRemoveLine
      StrCmp $4 "" unRemoveFromEnvVar_dosLoopEnd
      FileWrite $3 $4
      Goto unRemoveFromEnvVar_dosLoop
      unRemoveFromEnvVar_dosLoopRemoveLine:
        SetRebootFlag true
        Goto unRemoveFromEnvVar_dosLoop
 
    unRemoveFromEnvVar_dosLoopEnd:
      FileClose $3
      FileClose $2
      StrCpy $2 $WINDIR 2
      Delete "$2\autoexec.bat"
      CopyFiles /SILENT $5 "$2\autoexec.bat"
      Delete $5
      Goto unRemoveFromEnvVar_done
 
  unRemoveFromEnvVar_NT:
    ReadRegStr $2 ${WriteEnvStr_RegKey} $0
    StrCpy $6 $2 1 -1 # copy last char
    StrCmp $6 ";" +2 # if last char != ;
      StrCpy $2 "$2;" # append ;
    Push $2
    Push "$1;"
    Call un.StrStr ; Find `$1;` in $2
    Pop $3 ; pos of our dir
    StrCmp $3 "" unRemoveFromEnvVar_done
      ; else, it is in path
      # $1 - path to add
      # $2 - path var
      StrLen $4 "$1;"
      StrLen $5 $3
      StrCpy $6 $2 -$5 # $6 is now the part before the path to remove
      StrCpy $7 $3 "" $4 # $7 is now the part after the path to remove
      StrCpy $4 $6$7
 
      StrCpy $6 $4 1 -1 # copy last char
      StrCmp $6 ";" 0 +2 # if last char == ;
      StrCpy $4 $4 -1 # remove last char
 
      WriteRegExpandStr ${WriteEnvStr_RegKey} $0 $4
      SendMessage ${HWND_BROADCAST} ${WM_WININICHANGE} 0 "STR:Environment" /TIMEOUT=5000
 
  unRemoveFromEnvVar_done:
    Pop $7
    Pop $6
    Pop $5
    Pop $4
    Pop $3
    Pop $2
    Pop $1
	Pop $0
FunctionEnd
 
 
 
 
!ifndef IsNT_KiCHiK
!define IsNT_KiCHiK
 
###########################################
#            Utility Functions            #
###########################################
 
; IsNT
; no input
; output, top of the stack = 1 if NT or 0 if not
;
; Usage:
;   Call IsNT
;   Pop $R0
;  ($R0 at this point is 1 or 0)
 
!macro IsNT un
Function ${un}IsNT
  Push $0
  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" CurrentVersion
  StrCmp $0 "" 0 IsNT_yes
  ; we are not NT.
  Pop $0
  Push 0
  Return
 
  IsNT_yes:
    ; NT!!!
    Pop $0
    Push 1
FunctionEnd
!macroend
!insertmacro IsNT ""
!insertmacro IsNT "un."
 
!endif ; IsNT_KiCHiK
 
; StrStr
; input, top of stack = string to search for
;        top of stack-1 = string to search in
; output, top of stack (replaces with the portion of the string remaining)
; modifies no other variables.
;
; Usage:
;   Push "this is a long ass string"
;   Push "ass"
;   Call StrStr
;   Pop $R0
;  ($R0 at this point is "ass string")
 
!macro StrStr un
Function ${un}StrStr
Exch $R1 ; st=haystack,old$R1, $R1=needle
  Exch    ; st=old$R1,haystack
  Exch $R2 ; st=old$R1,old$R2, $R2=haystack
  Push $R3
  Push $R4
  Push $R5
  StrLen $R3 $R1
  StrCpy $R4 0
  ; $R1=needle
  ; $R2=haystack
  ; $R3=len(needle)
  ; $R4=cnt
  ; $R5=tmp
  loop:
    StrCpy $R5 $R2 $R3 $R4
    StrCmp $R5 $R1 done
    StrCmp $R5 "" done
    IntOp $R4 $R4 + 1
    Goto loop
done:
  StrCpy $R1 $R2 "" $R4
  Pop $R5
  Pop $R4
  Pop $R3
  Pop $R2
  Exch $R1
FunctionEnd
!macroend
!insertmacro StrStr ""
!insertmacro StrStr "un."
 
!endif ; _AddToPath_nsh 
 



;Function to check if Ferda is already installed on the computer
Function PreviousFerdaInstalled
   Push $0
   Push $1
   Push $2
  ; Push $3
   
   ;StrCpy $3 "0"
   
   StrCpy $0 "SOFTWARE\Ferda Dataminer" ;registry entry to search for Ferda

   ReadRegStr $1 HKCU "$0" "Version"
   
  ; MessageBox MB_OK "Verze $1"
   
   StrCmp $1 "" FerdaNotInstalled FerdaInstalled
   
   FerdaInstalled:
   StrCpy $2 $1
  ; MessageBox MB_OK "Installed"
   goto done
   
   FerdaNotInstalled:
   StrCpy $2 ""
 ;  MessageBox MB_OK "Not Installed"
   goto done
   
  ; Pop $3
  
  done:
   Pop $1
   Pop $0
   Exch $2     
   
FunctionEnd

Function GetFerdaPath

Push $0
   Push $1
   Push $2
  ; Push $3
   
   ;StrCpy $3 "0"
   
   StrCpy $0 "SOFTWARE\Ferda Dataminer" ;registry entry to search for Ferda

   ReadRegStr $1 HKCU "$0" "InstallDir"
   
  ; MessageBox MB_OK "Verze $1"
   
   StrCmp $1 "" FerdaNotInstalled FerdaInstalled
   
   FerdaInstalled:
   StrCpy $2 $1
  ; MessageBox MB_OK "Installed"
   goto done
   
   FerdaNotInstalled:
   StrCpy $2 ""
  ; MessageBox MB_OK "Not Installed"
   goto done
   
  ; Pop $3
  
  done:
   Pop $1
   Pop $0
   Exch $2     

FunctionEnd

; Usage
; Define in your script two constants:
;   DOT_MAJOR "(Major framework version)"
;   DOT_MINOR "{Minor framework version)"
;   DOT_MINOR_MINOR "{Minor framework version - last number after the second dot)"
; 
; Call IsDotNetInstalledAdv
; This function will abort the installation if the required version 
; or higher version of the .NETFramework is not installed.  Place it in
; either your .onInit function or your first install section before 
; other code.
Function IsDotNetInstalledAdv
   Push $0
   Push $1
   Push $2
   Push $3
   Push $4
   Push $5
   
  StrCpy $0 "0"
  StrCpy $1 "SOFTWARE\Microsoft\.NETFramework" ;registry entry to look in.
  StrCpy $2 0
  
      
 
  StartEnum:
    ;Enumerate the versions installed.
    EnumRegKey $3 HKLM "$1\policy" $2
    
    ;If we don't find any versions installed, it's not here.
    StrCmp $3 "" noDotNet notEmpty
    
    ;We found something.
    notEmpty:
      ;Find out if the RegKey starts with 'v'.  
      ;If it doesn't, goto the next key.
      StrCpy $4 $3 1 0
      StrCmp $4 "v" +1 goNext
      StrCpy $4 $3 1 1
      
      ;It starts with 'v'.  Now check to see how the installed major version
      ;relates to our required major version.
      ;If it's equal check the minor version, if it's greater, 
      ;we found a good RegKey.
      IntCmp $4 ${DOT_MAJOR} +1 goNext yesDotNetReg
      ;Check the minor version.  If it's equal or greater to our requested 
      ;version then we're good.
      StrCpy $4 $3 1 3
      IntCmp $4 ${DOT_MINOR} +1 goNext yesDotNetReg
      
      ;detect sub-version - e.g. 2.0.50727
      ;takes a value of the registry subkey - it contains the small version number
      EnumRegValue $5 HKLM "$1\policy\$3" 0
       
      IntCmpU $5 ${DOT_MINOR_MINOR} yesDotNetReg goNext yesDotNetReg
 
    goNext:
      ;Go to the next RegKey.
      IntOp $2 $2 + 1
      goto StartEnum
 
  yesDotNetReg: 
    ;Now that we've found a good RegKey, let's make sure it's actually
    ;installed by getting the install path and checking to see if the 
    ;mscorlib.dll exists.
    EnumRegValue $2 HKLM "$1\policy\$3" 0
    ;$2 should equal whatever comes after the major and minor versions 
    ;(ie, v1.1.4322)
    StrCmp $2 "" noDotNet
    ReadRegStr $4 HKLM $1 "InstallRoot"
    ;Hopefully the install root isn't empty.
    StrCmp $4 "" noDotNet
    ;build the actuall directory path to mscorlib.dll.
    StrCpy $4 "$4$3.$2\mscorlib.dll"
    IfFileExists $4 yesDotNet noDotNet
 
  noDotNet:
    ;Nope, something went wrong along the way.  Looks like the 
    ;proper .NETFramework isn't installed.  
    
    ;Uncomment the following line to make this function throw a message box right away 
   ; MessageBox MB_OK "You must have v${DOT_MAJOR}.${DOT_MINOR} or greater of the .NETFramework installed.  Aborting!"
   ; Abort
     StrCpy $0 0
     Goto done
     
     yesDotNet:
    ;Everything checks out.  Go on with the rest of the installation.
    StrCpy $0 1

   done:
     Pop $4
     Pop $3
     Pop $2
     Pop $1
     Exch $0
 FunctionEnd
 
 Function SaveIceDirectory
 Var /GLOBAL IcePath


 StrCpy $IcePath $INSTDIR
 ;Push $R2
 
 FunctionEnd

 Function InsDirectoryPre
 
 StrCpy $INSTDIR "c:\FerdaDataminer"
 
 FunctionEnd
 
 Function IceDirectoryPre
 
 StrCpy $INSTDIR "c:\Ice-3.0.1"
 
 FunctionEnd
  
 Function IsExactDotNetInstalled
   Push $0
   Push $1
   Push $2
   Push $3
   Push $4
   Push $5
   
  StrCpy $0 "0"
  StrCpy $1 "SOFTWARE\Microsoft\.NETFramework" ;registry entry to look in.
  StrCpy $2 0
 
  StartEnum:
    ;Enumerate the versions installed.
    EnumRegKey $3 HKLM "$1\policy" $2
    
    ;If we don't find any versions installed, it's not here.
    StrCmp $3 "" noDotNet notEmpty
    
    ;MessageBox MB_OK "$3"
    
    ;We found something.
    notEmpty:
      ;Find out if the RegKey starts with 'v'.  
      ;If it doesn't, goto the next key.
      StrCpy $4 $3 1 0
      StrCmp $4 "v" +1 goNext
      StrCpy $4 $3 1 1
      
      ;It starts with 'v'.  Now check to see how the installed major version
      ;relates to our required major version.
      ;If it's equal check the minor version, if it's greater, 
      ;we found a good RegKey.
      
      IntCmp $4 ${DOT_BAD_MAJOR} yesDotNet goNext goNext

 
    goNext:
      ;Go to the next RegKey.
      IntOp $2 $2 + 1
      goto StartEnum
 
 
  noDotNet:
    ;Nope, something went wrong along the way.  Looks like the 
    ;proper .NETFramework isn't installed.  
     StrCpy $0 0
     Goto done
     
     yesDotNet:
    ;Everything checks out.  Go on with the rest of the installation.
    StrCpy $0 1

   done:
     Pop $4
     Pop $3
     Pop $2
     Pop $1
     Exch $0
 FunctionEnd
    
 
 ;>>>>>> Function Junction BEGIN
;Original Written by Afrow UK
; Rewrite to Replace on line within text by rainmanx
; This version works on R4 and R3 of Nullsoft Installer
; It replaces whatever is in the line throughout the entire text matching it.
Function AdvReplaceInFile
Exch $0 ;file to replace in
Exch
Exch $1 ;number to replace after
Exch
Exch 2
Exch $2 ;replace and onwards
Exch 2
Exch 3
Exch $3 ;replace with
Exch 3
Exch 4
Exch $4 ;to replace
Exch 4
Push $5 ;minus count
Push $6 ;universal
Push $7 ;end string
Push $8 ;left string
Push $9 ;right string
Push $R0 ;file1
Push $R1 ;file2
Push $R2 ;read
Push $R3 ;universal
Push $R4 ;count (onwards)
Push $R5 ;count (after)
Push $R6 ;temp file name
;-------------------------------
GetTempFileName $R6
FileOpen $R1 $0 r ;file to search in
FileOpen $R0 $R6 w ;temp file
StrLen $R3 $4
StrCpy $R4 -1
StrCpy $R5 -1
loop_read:
ClearErrors
FileRead $R1 $R2 ;read line
IfErrors exit
StrCpy $5 0
StrCpy $7 $R2
loop_filter:
IntOp $5 $5 - 1
StrCpy $6 $7 $R3 $5 ;search
StrCmp $6 "" file_write2
StrCmp $6 $4 0 loop_filter
StrCpy $8 $7 $5 ;left part
IntOp $6 $5 + $R3
StrCpy $9 $7 "" $6 ;right part
StrLen $6 $7
StrCpy $7 $8$3$9 ;re-join
StrCmp -$6 $5 0 loop_filter
IntOp $R4 $R4 + 1
StrCmp $2 all file_write1
StrCmp $R4 $2 0 file_write2
IntOp $R4 $R4 - 1
IntOp $R5 $R5 + 1
StrCmp $1 all file_write1
StrCmp $R5 $1 0 file_write1
IntOp $R5 $R5 - 1
Goto file_write2
file_write1:
FileWrite $R0 $7 ;write modified line
Goto loop_read
file_write2:
FileWrite $R0 $7 ;write modified line
Goto loop_read
exit:
FileClose $R0
FileClose $R1
SetDetailsPrint none
Delete $0
Rename $R6 $0
Delete $R6
SetDetailsPrint both
;-------------------------------
Pop $R6
Pop $R5
Pop $R4
Pop $R3
Pop $R2
Pop $R1
Pop $R0
Pop $9
Pop $8
Pop $7
Pop $6
Pop $5
Pop $4
Pop $3
Pop $2
Pop $1
Pop $0
FunctionEnd
;>>>>>>>>>>>>> Function END
 

Function CheckForSpaces
 Push $R1
 Push $R2
 Push $R3
 Push $R4
 StrCpy $R1 -1
 StrCpy $R3 0
 loop:
   StrCpy $R2 $INSTDIR 1 $R1
   IntOp $R1 $R1 - 1
   StrCmp $R2 "" done
   StrCmp $R2 " " found
   StrCpy $R4 "$R2$R4"
 Goto loop
 found:
   IntOp $R3 $R3 + 1
 Goto loop
 done:
   StrCmp $R3 0 +3
   MessageBox MB_OK|MB_ICONEXCLAMATION "$(HasSpaces)"
   Abort
 Pop $R4
 Pop $R3
 Pop $R2
 Pop $R1
FunctionEnd