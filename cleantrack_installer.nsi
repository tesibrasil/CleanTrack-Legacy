!include WinMessages.nsh
# SetCompress off
SetCompressor /SOLID /FINAL lzma
!define VERSION_FULL "10.0.0.0"

BrandingText " "
ChangeUI all "${NSISDIR}\Contrib\UIs\default.exe"
CheckBitmap "${NSISDIR}\Contrib\Graphics\Checks\simple-round2.bmp"
Icon Res\Tesi.ico
InstallColors DC0049 9E9E9E
InstallDir C:\Cleantrack
InstallDirRegKey HKLM "Software\Cleantrack" "Install_Dir"
Name "Cleantrack"
OutFile "CleantrackSetup.exe"
RequestExecutionLevel admin
ShowInstDetails show
ShowUninstDetails show
XPStyle on
  
LoadLanguageFile "${NSISDIR}\Contrib\Language files\English.nlf"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\Italian.nlf"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\Portuguese.nlf"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\PortugueseBR.nlf"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\Spanish.nlf"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\SpanishInternational.nlf"

VIAddVersionKey /LANG=${LANG_ITALIAN} "ProductName" "CleantrackInstaller"
VIAddVersionKey /LANG=${LANG_ITALIAN} "CompanyName" "Tesi Elettronica e Sistemi Informativi S.p.A."
VIAddVersionKey /LANG=${LANG_ITALIAN} "FileDescription" "CleantrackInstaller"
VIAddVersionKey /LANG=${LANG_ITALIAN} "InternalName" "Cleantrack Installer"
VIAddVersionKey /LANG=${LANG_ITALIAN} "LegalCopyright" "Tesi Elettronica e Sistemi Informativi S.p.A."
VIAddVersionKey /LANG=${LANG_ITALIAN} "OriginalFilename" "Cleantrack Installer"
VIProductVersion "${VERSION_FULL}"

LangString Section2Name ${LANG_ENGLISH} "Link on desktop"
LangString Section2Name ${LANG_ITALIAN} "Collegamento sul desktop"
LangString Section2Name ${LANG_PORTUGUESE} "Link on desktop"
LangString Section2Name ${LANG_PORTUGUESEBR} "Link on desktop"
LangString Section2Name ${LANG_SPANISH} "Link on desktop"
LangString Section2Name ${LANG_SPANISHINTERNATIONAL} "Link on desktop"
LangString Section3Name ${LANG_ENGLISH} "Links on start menu"
LangString Section3Name ${LANG_ITALIAN} "Collegamenti nel menu di avvio"
LangString Section3Name ${LANG_PORTUGUESE} "Links on start menu"
LangString Section3Name ${LANG_PORTUGUESEBR} "Links on start menu"
LangString Section3Name ${LANG_SPANISH} "Links on start menu"
LangString Section3Name ${LANG_SPANISHINTERNATIONAL} "Links on start menu"
LangString Message0Name ${LANG_ENGLISH} "operating system not supported!"
LangString Message0Name ${LANG_ITALIAN} "sistema operativo non supportato!"
LangString Message0Name ${LANG_PORTUGUESE} "operating system not supported!"
LangString Message0Name ${LANG_PORTUGUESEBR} "operating system not supported!"
LangString Message0Name ${LANG_SPANISH} "operating system not supported!"
LangString Message0Name ${LANG_SPANISHINTERNATIONAL} "operating system not supported!"
LangString Message1Name ${LANG_ENGLISH} "Cleantrack configuration "
LangString Message1Name ${LANG_ITALIAN} "Cleantrack configurazione "
LangString Message1Name ${LANG_PORTUGUESE} "Cleantrack configuration "
LangString Message1Name ${LANG_PORTUGUESEBR} "Cleantrack configuration "
LangString Message1Name ${LANG_SPANISH} "Cleantrack configuration "
LangString Message1Name ${LANG_SPANISHINTERNATIONAL} "Cleantrack configuration "
# 
Page components
Page directory
Page instfiles
UninstPage uninstConfirm
UninstPage instfiles

# 
Function .onInit 
  ClearErrors
  Var /GLOBAL Var1
  Var /GLOBAL Var2
  ReadRegStr $Var1 HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" CurrentVersion
  IfErrors 0 lbl_winnt
  ReadRegStr $Var1 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion" VersionNumber
  StrCpy $Var2 $Var1 1
  StrCmp $Var2 '4' 0 lbl_error 
  StrCpy $Var2 $Var1 3 
  lbl_winnt:
    StrCpy $Var2 $Var1 1 
    StrCmp $Var2 '3' lbl_winnt_x
    StrCmp $Var2 '4' lbl_winnt_x 
    StrCpy $Var2 $Var1 3 
    StrCmp $Var2 '5.0' lbl_winnt_2000
    StrCmp $Var2 '5.1' lbl_winnt_XP
    StrCmp $Var2 '5.2' lbl_winnt_2003
    StrCmp $Var2 '6.0' lbl_winnt_vista
    StrCmp $Var2 '6.1' lbl_winnt_7
    StrCmp $Var2 '6.2' lbl_winnt_8
    StrCmp $Var2 '6.3' lbl_winnt_8_1 lbl_error 
    lbl_winnt_x:
      MessageBox MB_OK|MB_ICONSTOP "Windows NT: $(Message0Name)" IDOK 
      Quit
    lbl_winnt_2000:
      MessageBox MB_OK|MB_ICONSTOP "Windows 2000: $(Message0Name)" IDOK 
      Quit
    lbl_winnt_XP:
      Goto lbl_done
    lbl_winnt_2003:
      Goto lbl_done
    lbl_winnt_vista:
      Goto lbl_done
    lbl_winnt_7:
      Goto lbl_done
    lbl_winnt_8:
      Goto lbl_done
    lbl_winnt_8_1:
      Goto lbl_done
    lbl_error:
      MessageBox MB_OK|MB_ICONSTOP "$(Message0Name)" IDOK 
      Quit
  lbl_done:
FunctionEnd

Section "Prerequisiti"
  SectionIn 1
  SetShellVarContext all	; $DESKTOP e $SMPROGRAMS (utilizzati per creare i link) puntano a "ALL USERS"
  SetOutPath $INSTDIR
  
  ; .NET Framework 4.8
  File "Prerequisites\ndp48-x86-x64-allos-enu.exe"
  ExecWait "$INSTDIR\ndp48-x86-x64-allos-enu.exe /norestart /passive"
  Delete   "$INSTDIR\ndp48-x86-x64-allos-enu.exe"

  ; vc redist x86 up to 2019
  File "Prerequisites\VC_redist.x86.exe"
  ExecWait "$INSTDIR\VC_redist.x86.exe /norestart /passive"
  Delete   "$INSTDIR\VC_redist.x86.exe"
  
SectionEnd

Section "Client"
  SectionIn 2
  SetShellVarContext all
  SetOutPath $INSTDIR
  CreateDirectory $INSTDIR\Client
  CreateDirectory $INSTDIR\Client\log
  CreateDirectory $INSTDIR\ConfigUI
  
  SetOutPath $INSTDIR\Client
  File /r .\KleanTrak\bin\x86\Release\*.exe
  File /r .\KleanTrak\bin\x86\Release\*.dll
  File /r .\KleanTrak\bin\x86\Release\*.config
  ; File /r .\KleanTrak\bin\x86\Release\*.xml
  ; File /r .\KleanTrak\bin\x86\Release\*.bin
  File /r .\KleanTrak\bin\x86\Release\*.mdb
  File /r .\KleanTrak\bin\x86\Release\*.pdb
  
  SetOutPath $INSTDIR\ConfigUI
  File /r .\ConfigUI\bin\Release\*.*
  
  ; scrivo nel registro tutto quello che mi serve per la disinstallazione
  WriteRegStr HKLM SOFTWARE\Cleantrack "Install_Dir" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "DisplayName" "Cleantrack"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "UninstallString" '"$INSTDIR\Cleantrack_Uninstaller.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "NoRepair" 1
  
  ; creo il file di disinstallazione
  WriteUninstaller "Cleantrack_Uninstaller.exe"  
  
  ExecWait '$INSTDIR\ConfigUI\ConfigUI.exe -c "$INSTDIR\Client\kleantrak.ini"'
SectionEnd


Section "Server"
  SectionIn 3
  SetShellVarContext all
  SetOutPath $INSTDIR
  CreateDirectory $INSTDIR\Server
  CreateDirectory $INSTDIR\Server\log
  CreateDirectory $INSTDIR\ConfigUI
  
  SetOutPath $INSTDIR\Server
  File /r .\KleanTrak.Server\bin\Release\*.exe
  File /r .\KleanTrak.Server\bin\Release\*.dll
  File /r .\KleanTrak.Server\bin\Release\*.config
  File /r .\KleanTrak.Server\bin\Release\*.xml
  File /r .\KleanTrak.Server\bin\Release\*.pdb
  
  SetOutPath $INSTDIR\ConfigUI
  File /r .\ConfigUI\bin\Release\*.*
  
  ; scrivo nel registro tutto quello che mi serve per la disinstallazione
  WriteRegStr HKLM SOFTWARE\Cleantrack "Install_Dir" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "DisplayName" "Cleantrack"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "UninstallString" '"$INSTDIR\Cleantrack_Uninstaller.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "NoRepair" 1
  
  ; creo il file di disinstallazione
  WriteUninstaller "Cleantrack_Uninstaller.exe"
  
  ExecWait '$INSTDIR\Server\KleanTrak.Server.exe -i'
  
  ExecWait '$INSTDIR\ConfigUI\ConfigUI.exe -s "$INSTDIR\Server\Settings.cfg"'
  
SectionEnd

Section "Database scripts"
  SectionIn 4
  SetShellVarContext all
  SetOutPath $INSTDIR
  CreateDirectory $INSTDIR\DbScripts
  
  SetOutPath $INSTDIR\DbScripts
  File /r .\Database\*.*
  
  ; scrivo nel registro tutto quello che mi serve per la disinstallazione
  WriteRegStr HKLM SOFTWARE\Cleantrack "Install_Dir" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "DisplayName" "Cleantrack"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "UninstallString" '"$INSTDIR\Cleantrack_Uninstaller.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack" "NoRepair" 1
  
  ; creo il file di disinstallazione
  WriteUninstaller "Cleantrack_Uninstaller.exe"    
SectionEnd

# 
Section "Uninstall"  
  ExecWait 'SC STOP "Cleantrack"'
  ExecWait 'SC DELETE "Cleantrack"'
  SetShellVarContext all
  DeleteRegKey HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\Cleantrack
  DeleteRegKey HKLM Software\Cleantrack
  SetOutPath "C:\"
  RMDir /r $INSTDIR
SectionEnd
