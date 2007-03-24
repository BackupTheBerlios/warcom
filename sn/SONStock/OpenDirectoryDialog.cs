using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SONStock
{
public class OpenDirectoryDialog : FolderNameEditor
{
    private FolderBrowser folderDialog;
    public OpenDirectoryDialog()
    {
        folderDialog = new FolderBrowser();

        // if no changes to defaults, skip using this function
        this.Initialize( );
    }

    public string GetFolder()
    {
        folderDialog.StartLocation = FolderBrowserFolder.Desktop;
        folderDialog.Style = FolderNameEditor.FolderBrowserStyles.ShowTextBox;
        System.Windows.Forms.DialogResult rs = folderDialog.ShowDialog();
            return folderDialog.DirectoryPath;
    }

    protected void Initialize( )
    {
        base.InitializeDialog( folderDialog );
        // Examples of making initialization changes.
        folderDialog.StartLocation = FolderBrowserFolder.MyComputer;
        folderDialog.Style = FolderNameEditor.FolderBrowserStyles.ShowTextBox;
    }
}
}
