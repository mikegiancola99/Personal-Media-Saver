!include "MultiUser.nsh"
!include "MUI2.nsh"
!include "Sections.nsh"
!include "LogicLib.nsh"
!include "Memento.nsh"
!include "WordFunc.nsh"
!include "nsDialogs.nsh"
!include "x64.nsh"

!define PATH_TOOLBAR 'Toolbar'
!define PATH_BASE 'Output'
!define PATH_SQL_BITS '3rdParty\\SqlServerCE'

!define MUI_ABORTWARNING

Name "Personal Picture Manager Installer"
Caption "Personal Picture Manager Installer"

OutFile "PersonalPictureManagerInstaller.exe"
InstallDir "$PROGRAMFILES\PersonalPictureManager"
RequestExecutionLevel admin

# license page
!insertmacro MUI_PAGE_LICENSE "${PATH_TOOLBAR}\Toolbar_Eula.rtf"
!define MUI_COMPONENTSPAGE_SMALLDESC
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

# ------------------------------------------------------------ #
# Set descriptions install section
# ------------------------------------------------------------ #
!define INSTDIR_REG_ROOT "HKLM"
!define INSTDIR_REG_KEY  "Software\Microsoft\Windows\CurrentVersion\Uninstall\Personal Picture Manager"
!define MEMENTO_REGISTRY_ROOT ${INSTDIR_REG_ROOT}
!define MEMENTO_REGISTRY_KEY "${INSTDIR_REG_KEY}" "Personal Picture Manager" "$INSTDIR\Uninstall.exe"

${MementoSection} "Personal Picture Manager Core Files (required)" SecCore
	SetDetailsPrint textonly
	DetailPrint "Installing Personal Picture Manager Core Files..."
	SetDetailsPrint listonly

	SetOutPath $INSTDIR
	
	File "${PATH_SQL_BITS}\SSCERuntime_x64-ENU.msi"
	File "${PATH_SQL_BITS}\SSCERuntime_x86-ENU.msi"
	
	${If} ${RunningX64}
		ExecWait 'msiexec /i "$INSTDIR\SSCERuntime_x64-ENU.msi" /q'
	${EndIf}
	ExecWait 'msiexec /i "$INSTDIR\SSCERuntime_x86-ENU.msi" /q'		
	
	
	File "${PATH_BASE}\PersonalPictureManagerService.exe"
	File "${PATH_BASE}\PersonalPictureManager.exe"
	File "${PATH_BASE}\Loading.ico";
	SetOutPath $INSTDIR

	WriteUninstaller $INSTDIR\Uninstall.exe
	
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Personal Picture Manager" "DisplayName" "Personal Picture Manager"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Personal Picture Manager" "DisplayIcon" "$\"$INSTDIR\PersonalPictureManager.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Personal Picture Manager" "Publisher" "JustGoodCode.com"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Personal Picture Manager" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""

	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "PersonalPictureManager" "$\"$INSTDIR\PersonalPictureManagerService.exe$\""

	SectionIn 1 RO
${MementoSectionEnd}


${MementoSection} "Create Start Menu and Desktop Shortcuts" SecShortcut
	CreateDirectory "$SMPROGRAMS\Personal Picture Manager"
	CreateShortCut  "$SMPROGRAMS\Personal Picture Manager\Persaonal Picture Manager.lnk" "$INSTDIR\PersonalPictureManager.exe" ""
	CreateShortCut  "$SMPROGRAMS\Personal Picture Manager\Uninstall.lnk" "$INSTDIR\Uninstall.exe" ""
    SectionIn 1
${MementoSectionEnd}

${MementoSectionDone}
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SecCore} "The core files required to use Personal Picture Manager"
  !insertmacro MUI_DESCRIPTION_TEXT ${SecShortcut} "Create Start Menu and Desktop Shortcuts"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Section "Uninstall"
	SetDetailsPrint textonly
	DetailPrint "Uninstalling Personal Picture Manager..."
	SetDetailsPrint listonly

	Exec '"$INSTDIR\PersonalPictureManagerService.exe" shutdown'

	Delete "$INSTDIR\PersonalPictureManagerService.exe"
	Delete "$INSTDIR\PersonalPictureManager.exe"
	Delete "$APPDATA\PersonalPictureManager\PersonalPictureManagerDB.sdf"
	Delete "$APPDATA\PersonalPictureManager\log.txt"
	Delete "$SMPROGRAMS\Personal Picture Manager\Persaonal Picture Manager.lnk"
	Delete "$SMPROGRAMS\Personal Picture Manager\Uninstall.lnk"

	RMDir $INSTDIR
	RMDir "$SMPROGRAMS\Personal Picture Manager"
	RMDir "$APPDATA\PersonalPictureManager"

	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Personal Picture Manager" 
	DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run\" "PersonalPictureManager"
	
	RMDir /R $INSTDIR
SectionEnd