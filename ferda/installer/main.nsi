;DICLAIMER:
;Work of installer DOES depend on order of functions used. If changing the function order, expect
;misbehaviour. Especially try not to touch localization stuff, for unknown reason it works only when
;given to this part of the code. You have been warned.

;use modern ui
!include "MUI.nsh"

;!include "Registry.nsh"

;include constants
!include "constants.nsi" 

;mainly 3-rd part functions
!include "functions.nsi"

;funcitons for uninstaller
!include "uninstall_functions.nsi"


XPStyle on

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\LICENSE"
!insertmacro MUI_PAGE_COMPONENTS
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE "CheckForSpaces"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH







;Default installation folder
InstallDir "C:\FerdaDataminer"
Name "Ferda DataMiner"
OutFile "ferda_install.exe"


;Langugage stuff 
!insertmacro MUI_LANGUAGE "Czech"
!insertmacro MUI_LANGUAGE "English"

;language strings
!include "localization.cs-CZ.nsi"  
!include "localization.en-US.nsi"


;Installer Sections
!include "oninit.nsi"

;!include "section_boxes.nsi"

;What to install - divided into sections
!include "section_basefiles.nsi"
!include "section_addins.nsi"
!include "section_boxes.nsi"
!include "section_ferdagens.nsi"

;Intergration section
!include "section_integration.nsi"





;Assign language strings to sections
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${BaseFiles} $(DESC_BaseFiles)
!insertmacro MUI_DESCRIPTION_TEXT ${FerdaAddIns} $(DESC_FerdaAddIns)
!insertmacro MUI_DESCRIPTION_TEXT ${AttributeFrequency} $(DESC_AttributeFrequency)
!insertmacro MUI_DESCRIPTION_TEXT ${ColumnFrequency} $(DESC_ColumnFrequency)
!insertmacro MUI_DESCRIPTION_TEXT ${DatabaseInfo} $(DESC_DatabaseInfo)
!insertmacro MUI_DESCRIPTION_TEXT ${ExplainTable} $(DESC_ExplainTable)
!insertmacro MUI_DESCRIPTION_TEXT ${EditCategories} $(DESC_EditCategories)
!insertmacro MUI_DESCRIPTION_TEXT ${ResultBrowser} $(DESC_ResultBrowser)
!insertmacro MUI_DESCRIPTION_TEXT ${ODBCConnectionString} $(DESC_ODBCConnectionString)
!insertmacro MUI_DESCRIPTION_TEXT ${Boxes} $(DESC_Boxes)
!insertmacro MUI_DESCRIPTION_TEXT ${FFTTask} $(DESC_FFTTAsk)
!insertmacro MUI_DESCRIPTION_TEXT ${KLTask} $(DESC_KLTask)
!insertmacro MUI_DESCRIPTION_TEXT ${SDFFTTask} $(DESC_SDFFTTask)  
!insertmacro MUI_DESCRIPTION_TEXT ${Ice} $(DESC_Ice)
!insertmacro MUI_DESCRIPTION_TEXT ${FerdaBase} $(DESC_FerdaBase)
!insertmacro MUI_DESCRIPTION_TEXT ${FerdaGens} $(DESC_FerdaGens)
!insertmacro MUI_DESCRIPTION_TEXT ${FerdaIntegration} $(DESC_FerdaIntegration)
!insertmacro MUI_DESCRIPTION_TEXT ${StartMenu} $(DESC_StartMenu)
!insertmacro MUI_DESCRIPTION_TEXT ${DesktopShortcut} $(DESC_DesktopShortcut)
!insertmacro MUI_DESCRIPTION_TEXT ${FileAssociation} $(DESC_FileAssociation)
!insertmacro MUI_FUNCTION_DESCRIPTION_END


;Uninstaller Section
!include "uninstall.nsi"
