using Terminal.Gui;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class ViewListProduct : Window
    {
        private int pageLength = 10;
        private int numPage = 1;
        private Label page;
        private Label totalPage;
        private ListView allProdListView;
        private ProductRepository productRepository;
        private OrderRepository orderRepository;
        private CheckRepository checkRepository;
        private User currentUser;
        private TextField searchInput;
        private string searchValue = "";
        public ViewListProduct()
        {
            Label search = new Label(2, 2, "Search: ");
            searchInput = new TextField(){X = Pos.Right(search) + 2, Y = Pos.Top(search), Width = 30, Text = ""};
            Button searchBut = new Button(){X = Pos.Right(searchInput) + 2, Y = Pos.Top(search),Text = "Search"};
            searchBut.Clicked += OnSearchPress;
            this.Add(search, searchInput, searchBut);

            this.Title = "List of product";
            
            Label textP = new Label("Total page:"){X = 2, Y = 4, Width = 15};
            totalPage = new Label("?")
            {
                X= Pos.Right(textP) + 2,
                Y = 4,
                Width = 5
            };
            this.allProdListView = new ListView(new List<Product>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            this.allProdListView.OpenSelectedItem += OnOpenProduct;
            FrameView frameView = new FrameView("Product:")
            {
                X = 2,
                Y = 8,
                Width = Dim.Fill() - 4,
                Height = pageLength + 1
            };
            Button prev = new Button(2, 20, "Prev");
            prev.Clicked += OnClickPrev;
            page = new Label("?")
            {
                X = Pos.Right(prev) + 3,
                Y = 20
            };
            Button next = new Button()
            {
                X = Pos.Right(page) + 3 ,
                Y = 20,
                Text = "Next"
            };
            next.Clicked += OnClickNext;
            frameView.Add(this.allProdListView);
            this.Add(textP,totalPage, frameView);
            this.Add(prev, page, next);
            Button back = new Button(){X = 3, Y = Pos.Bottom(prev) + 3, Text = "Back to main window"};
            back.Clicked += WindowBackToMain;
            this.Add(back);
        }
        public ListView GetProdList()
        {
            return this.allProdListView;
        }
        private void WindowBackToMain()
        {
            Application.RequestStop();
        }
        private void OnClickPrev()
        {
            if(numPage == 1)
            {   
                return;
            }
            this.numPage -= 1;
            ShowCurrentPage();
        }
        private void OnClickNext()
        {
            int totalP = productRepository.GetSearchPages(pageLength, searchValue);
            if(numPage >= totalP)
            {   
                return;
            }
            this.numPage += 1;
            ShowCurrentPage();

        }
        private void OnOpenProduct(ListViewItemEventArgs args)
        {
            ViewProduct viewWindow = new ViewProduct()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            Product pro = (Product)args.Value;
            viewWindow.SetProduct(pro);
            viewWindow.SetUser(currentUser);
            viewWindow.SetRepos(orderRepository, checkRepository, productRepository);
            Application.Run(viewWindow);
            if(viewWindow.exit)
            {
                checkClickProduct(viewWindow, pro.id);
            }
        }
        public void SetRepositories(ProductRepository repo,
        CheckRepository checks,
        OrderRepository ords,
        User user)
        {
            this.productRepository = repo;
            this.checkRepository = checks;
            this.orderRepository = ords;
            this.currentUser = user;
            this.ShowCurrentPage();
        }
        
        private void ShowCurrentPage()
        {
            this.page.Text = numPage.ToString();
            this.totalPage.Text = productRepository.GetSearchPages(pageLength, searchValue).ToString();
            List<Product> searchPr = productRepository.GetSearchPage(searchValue, numPage, pageLength);
            if(searchPr.Count == 0)
            {
                List<string> empty = new List<string>()
                {
                    "The search has not given any results. Enter another substring"
                };
                this.allProdListView.SetSource(empty);
            }
            else
            {
                this.allProdListView.SetSource(searchPr);
            }
        }
        public void checkClickProduct(ViewProduct viewWindow, long proId)
        {   
            if(viewWindow.del)
            {
                int resCheck = checkRepository.DeleteByProductId(proId);
                int res = productRepository.DeleteById(proId);
                if(res == 1)
                {
                    int pages = productRepository.GetTotalPages(pageLength);
                    if(numPage > pages)
                    {
                        numPage -= 1;
                    }
                    if(numPage == 0)
                    {
                        MessageBox.ErrorQuery("List of product is empty", "OK");
                    }
                    allProdListView.SetSource(productRepository.GetSearchPage(searchValue, numPage, pageLength));
                }
                else
                {
                    MessageBox.ErrorQuery("Delete product","Product cannot be deleted", "OK");
                }
            }
            if(viewWindow.edited)
            {
                int res = productRepository.UpdateById(proId, viewWindow.GetProduct());
                if(res == 1)
                {
                    allProdListView.SetSource(productRepository.GetSearchPage(searchValue, numPage, pageLength));
                }
                else
                {
                    MessageBox.ErrorQuery("Edit product","Product cannot be edited", "OK");
                }
            }
            
        }
        private void OnSearchPress()
        {
            this.searchValue = this.searchInput.Text.ToString(); 
            this.ShowCurrentPage();
        }
    }
}