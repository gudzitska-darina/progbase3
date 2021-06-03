using Terminal.Gui;
using System.Collections.Generic;

public class ImageGenerateWindow : Window
{
    private TextField fileInput;
    private ProductRepository repository;
    public ImageGenerateWindow()
    {
        this.Title = "Generate histogram";
        Label fileImp = new Label(3, 3, "Enter file:");
        this.fileInput = new TextField (){X = Pos.Right(fileImp) + 3, Y = 3, Width = 20, Text = ""};
        Button impB = new Button()
        {
            X = Pos.Right(fileInput) + 3,
            Y = 3,
            Text = "Confirm"
        };
        impB.Clicked += DoGenerate;
        this.Add(fileImp, fileInput,  impB);

        Button back = new Button(){X = 3, Y = Pos.Bottom(fileImp) + 5, Text = "Back to main window"};
        back.Clicked += WindowBackToMain;
        this.Add(back);
    }
    private void WindowBackToMain()
    {
        Application.RequestStop();
    }
    public void SetRepo(ProductRepository prods)
    {
        this.repository = prods;
    }
    private void DoGenerate()
    {
        if(string.IsNullOrEmpty(fileInput.Text.ToString()))
        {
            MessageBox.ErrorQuery("Export","File field cannot be empty", "OK");
            return;
        }
        else
        {
            ImageGeneration generete = new ImageGeneration();
            generete.SetFile(fileInput.Text.ToString());
            List<Product> prod = repository.GetAll();
            generete.Generate(prod);
            MessageBox.Query("Histogram","Success\nThe created histogram has been saved","Ok");
            Application.RequestStop();
        }
    }
    
}
