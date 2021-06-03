using Terminal.Gui;
using System.Collections.Generic;
public class ExportWindow : Window
{
    private TextField fileExpInput;
    private ProductRepository repository;
    private TextField exportInput;
    public ExportWindow()
    {
        this.Title = "Export";
        Label textExp = new Label(3, 3, "Enter substring");
        this.exportInput = new TextField (){X = Pos.Right(textExp) + 3, Y = 3, Width = 20, Text = ""};
        Label fileExp = new Label(3, 6, "Enter file:");
        this.fileExpInput = new TextField (){X = Pos.Right(fileExp) + 3, Y = 6, Width = 20, Text = ""};
        Button exp = new Button()
        {
            X = Pos.Right(exportInput) + 3,
            Y = 3,
            Text = "Confirm"
        };
        exp.Clicked += DoExport;
        this.Add(textExp, exportInput, fileExp, fileExpInput,  exp);

        Button back = new Button(){X = 3, Y = Pos.Bottom(fileExp) + 5, Text = "Back to main window"};
        back.Clicked += WindowBackToMain;
        this.Add(back);
    }
    private void WindowBackToMain()
    {
        Application.RequestStop();
    }
    public void SetProductRepository(ProductRepository repository)
    {
        this.repository = repository;
    }

    private void DoExport()
    {
        
        if(string.IsNullOrEmpty(exportInput.Text.ToString()))
        {
            MessageBox.ErrorQuery("Export","Substring cannot be empty", "OK");
            return;
        }
        else if(string.IsNullOrEmpty(fileExpInput.Text.ToString()))
        {
            MessageBox.ErrorQuery("Export","File field cannot be empty", "OK");
            return;
        }
        else
        {
            LibProduct expPro = repository.Export(exportInput.Text.ToString());
            if(expPro.products.Count == 0)
            {
                MessageBox.ErrorQuery("Export","No such products", "OK");
                return;
            }
            else
            {
                Export export = new Export(expPro, fileExpInput.Text.ToString());
                export.Serialize();
                MessageBox.Query("Export","Success\nThe created file has been saved","Ok");
                Application.RequestStop();
            }
        } 
    }
}
