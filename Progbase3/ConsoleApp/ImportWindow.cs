using Terminal.Gui;
using System.IO;
using System.Collections.Generic;
namespace ConsoleApp
{
    public class ImportWindow : Window
    {
        private TextField fileImpInput;
        private ProductRepository repository;
        public ImportWindow()
        {
            this.Title = "Import";
            Label fileImp = new Label(3, 3, "Enter file:");
            this.fileImpInput = new TextField (){X = Pos.Right(fileImp) + 3, Y = 3, Width = 20, Text = ""};
            Button impB = new Button()
            {
                X = Pos.Right(fileImpInput) + 3,
                Y = 3,
                Text = "Confirm"
            };
            impB.Clicked += DoImport;
            this.Add(fileImp, fileImpInput,  impB);

            Button back = new Button(){X = 3, Y = Pos.Bottom(fileImp) + 5, Text = "Back to main window"};
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
        private void DoImport()
        {
            if(File.Exists(fileImpInput.Text.ToString()))
            {
                Import import = new Import();
                import.SetFile(fileImpInput.Text.ToString());
                LibProduct libProd =  import.Deserialize();
                if(libProd .products.Count == 0)
                {
                    MessageBox.ErrorQuery("Error Import","The file structure does not match the database structure ", "OK");
                    return;
                }
                else
                {
                    foreach(var item in libProd.products)
                    {
                        if(item.name == null)
                        {
                            MessageBox.ErrorQuery("Error Import","The file structure does not match the database structure ", "OK");
                            break;
                        }
                        else
                        {
                            repository.Insert(item);
                    
                            MessageBox.Query("Import","Success\nNew items have been added to the database","Ok");
                            Application.RequestStop();
                        } 
                    }  
                }
            }
            else
            {
                MessageBox.ErrorQuery("Error Import","Import file not found", "OK");
            }  

        }
    }
}