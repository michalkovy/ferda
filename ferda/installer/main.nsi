;DICLAIMER:
;Work of installer DOES depend on order of functions used. If changing the function order, expect
;misbehaviour. Especially try not to touch localization stuff, for unknown reason it works only when
;given to this part of the code. You have been warned.

;use modern ui
!include "MUI.nsh"

;!include "Registry.nsh"

;include constants
!include "constants.nsi" 
;!define MUI_PRODUCT "FerdaDataminer"
;!define MUI_VERSION "2.2"
;mainly 3-rd part functions
!include "functions.nsi"

;funcitons for uninstaller
!include "uninstall_functions.nsi"

!include "FileFunc.nsh"



XPStyle on
!define MUI_ICON "icons\FerdaFrontEnd.ico"
!define MUI_UNICON "icons\FerdaFrontEnd.ico"
!define MUI_HEADERIMAGE_BITMAP "icons\ferdaheader.bmp"
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\LICENSE"
!insertmacro MUI_PAGE_COMPONENTS
!define MUI_DIRECTORYPAGE_TEXT_TOP "$(MUI_DIRECTORYPAGE_TEXT_TOP_B)"
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE "SaveIceDirectory"
!define MUI_PAGE_CUSTOMFUNCTION_PRE "IceDirectoryPre"
!insertmacro MUI_PAGE_DIRECTORY

!define MUI_PAGE_CUSTOMFUNCTION_PRE "InsDirectoryPre"
;!define MUI_PAGE_CUSTOMFUNCTION_LEAVE "CheckForSpaces"
!define MUI_DIRECTORYPAGE_TEXT_TOP "$(MUI_DIRECTORYPAGE_TEXT_TOP_A)"
!insertmacro MUI_PAGE_DIRECTORY

!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

;Default installation folder
;InstallDir "c:\FerdaDataminer"
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

;Intergration section
!include "section_integration.nsi"

;Assign language strings to sections
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${BaseFiles} $(DESC_BaseFiles)
!insertmacro MUI_DESCRIPTION_TEXT ${FerdaAddIns} $(DESC_FerdaAddIns)

;Assign language strings to add-ins
!insertmacro MUI_DESCRIPTION_TEXT ${ResultBrowser} $(DESC_ResultBrowser)
!insertmacro MUI_DESCRIPTION_TEXT ${ODBCConnectionString} $(DESC_ODBCConnectionString)
!insertmacro MUI_DESCRIPTION_TEXT ${MultiSelectStrings} $(DESC_MultiSelectStrings)
!insertmacro MUI_DESCRIPTION_TEXT ${FormEditor} $(DESC_FormEditor)
!insertmacro MUI_DESCRIPTION_TEXT ${FormGenerator} $(DESC_FormGenerator)
!insertmacro MUI_DESCRIPTION_TEXT ${SetOntologyMapping} $(DESC_SetOntologyMapping)
!insertmacro MUI_DESCRIPTION_TEXT ${SetOntologyPath} $(DESC_SetOntologyPath)
!insertmacro MUI_DESCRIPTION_TEXT ${EditCategories} $(DESC_EditCategories)
!insertmacro MUI_DESCRIPTION_TEXT ${DatabaseInfo} $(DESC_DatabaseInfo)
!insertmacro MUI_DESCRIPTION_TEXT ${ExplainTable} $(DESC_ExplainTable)
!insertmacro MUI_DESCRIPTION_TEXT ${FrequencyDisplayer} $(DESC_FrequencyDisplayer)
!insertmacro MUI_DESCRIPTION_TEXT ${ShowTable} $(DESC_ShowTable)

!insertmacro MUI_DESCRIPTION_TEXT ${DataPreparation} $(DESC_DataPreparation)
!insertmacro MUI_DESCRIPTION_TEXT ${GuhaMining} $(DESC_GuhaMining)
!insertmacro MUI_DESCRIPTION_TEXT ${Sample} $(DESC_Sample)
!insertmacro MUI_DESCRIPTION_TEXT ${OntologyRelated} $(DESC_OntologyRelated)
!insertmacro MUI_DESCRIPTION_TEXT ${Language} $(DESC_Language)
!insertmacro MUI_DESCRIPTION_TEXT ${Wizards} $(DESC_Wizards)

!insertmacro MUI_DESCRIPTION_TEXT ${Boxes} $(DESC_Boxes)

!insertmacro MUI_DESCRIPTION_TEXT ${FerdaBase} $(DESC_FerdaBase)

!insertmacro MUI_DESCRIPTION_TEXT ${FerdaIntegration} $(DESC_FerdaIntegration)
!insertmacro MUI_DESCRIPTION_TEXT ${StartMenu} $(DESC_StartMenu)
!insertmacro MUI_DESCRIPTION_TEXT ${DesktopShortcut} $(DESC_DesktopShortcut)
!insertmacro MUI_DESCRIPTION_TEXT ${FileAssociation} $(DESC_FileAssociation)
!insertmacro MUI_FUNCTION_DESCRIPTION_END


;Uninstaller Section
!include "uninstall.nsi"
