using System;
using Terminal.Gui;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class MainWindow: Window
    {
        private int pageLength = 10;
        private int numPage = 1;
        private User currentUser;
        private Label currentUserView;
        private ListView allProdListView;
        private ListView allOrdListView;
        private ProductRepository productRepository;
        private OrderRepository orderRepository;
        private CheckRepository checkRepository;

        public MainWindow()
        {
            MenuBar menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Import", "", OnClickedImport),
                    new MenuItem ("_Export", "", OnClickedExport),
                    new MenuItem ("_Image", "", OnClickedImage),
                    new MenuItem ("_Quit", "", OnQuit)
                }),
            });
            this.Add(menu);

            Label textU = new Label("Current user:"){X = Pos.Percent(60), Y = Pos.Percent(98), Width = 15};
            currentUserView = new Label("?")
            {
                X = Pos.Percent(75),
                Y = Pos.Percent(98),
                Width = 10,
                
            };
            Button userB = new Button(){X = Pos.Percent(85), Y = Pos.Percent(98), Text = "View all info"};
            userB.Clicked += OnClickedViewInfo;
            
            Button addNewPro = new Button(1, 3, "Add new Product");
            addNewPro.Clicked += OnClickedAddingPro;
            this.Add(addNewPro, currentUserView, textU, userB);

            Button viewAllPro = new Button(1, 5, "View All Product");
            viewAllPro.Clicked += OnClickedViewListPro;
            Button viewAllOrd = new Button(){X= Pos.Right(viewAllPro) + 2, Y = 5, Text = "View All Order"};
            viewAllOrd.Clicked += OnClickedViewListOrd;
            this.Add(viewAllPro, viewAllOrd);

        }
        public void SetOrderRepository(OrderRepository repo)
        {
            this.orderRepository  = repo;
        }
        public void SetProductRepository(ProductRepository repo)
        {
            this.productRepository  = repo;
        }
        public void SetCheckRepository(CheckRepository repo)
        {
            this.checkRepository  = repo;
        }
        public void SetUser(User user)
        {
            this.currentUser = user;
            this.currentUser.orders = orderRepository.GetByUserId(user.id);
            this.currentUserView.Text = user.login;
        }
        
        private void OnClickedViewInfo()
        {
            UserInfoDialog info = new UserInfoDialog();
            info.SetUser(this.currentUser);
            Application.Run(info);
        }
        private void OnClickedExport()
        {
            ExportWindow winExp = new ExportWindow()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            winExp.SetProductRepository(productRepository);
            Application.Run(winExp);
        }
        private void OnClickedImport()
        {
            ImportWindow winImp = new ImportWindow()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            winImp.SetProductRepository(productRepository);
            Application.Run(winImp);
        }
        private void OnClickedAddingPro()
        {
            if(currentUser.status == "moderator")
            {
                ViewListProduct winListPro = new ViewListProduct();
                this.allProdListView = winListPro.GetProdList();

                CreateProduct windowAdd = new CreateProduct()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                Application.Run(windowAdd);

                if(windowAdd.status)
                {
                    Product newPro = windowAdd.GetProduct();
                    long proId = productRepository.Insert(newPro);
                    newPro.id = proId;
                    allProdListView.SetSource(productRepository.GetPage(numPage, pageLength));

                
                    ViewProduct viewWindow = new ViewProduct()
                    {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                    };
                    viewWindow.SetProduct(productRepository.GetByName(newPro.name));
                    viewWindow.SetUser(currentUser);
                    Application.Run(viewWindow); 
                }
            }
            else
            {
                MessageBox.ErrorQuery("Adding product","Only moderator can add products", "OK");
            }
        }
        private void OnClickedViewListPro()
        {
            ViewListProduct winListPro = new ViewListProduct();
            winListPro.SetRepositories(productRepository, checkRepository, orderRepository, currentUser);
            Application.Run(winListPro);

            this.allProdListView = winListPro.GetProdList();
        }
        private void OnClickedViewListOrd()
        {
            ViewListOrder winListOrd = new ViewListOrder();
            winListOrd.SetOrderRepository(orderRepository, checkRepository, productRepository);
            Application.Run(winListOrd);

            this.allOrdListView = winListOrd.GetOrdList();
        }

        private void OnQuit()
        {
            Application.RequestStop();
        }  

        private void OnClickedImage()
        {
            ImageGenerateWindow image = new ImageGenerateWindow();
            image.SetRepo(productRepository);
            Application.Run(image);
        }
        
    }
}