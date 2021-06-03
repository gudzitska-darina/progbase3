using Terminal.Gui;
using System.Linq;

namespace ConsoleApp
{
    public class ViewProduct : Window
    {
        public bool del;
        public bool edited;
        protected Product product;
        private OrderRepository orderRepository;
        private CheckRepository checkRepository;
        private ProductRepository productRepository;
        private TextField idInput;
        protected TextField nameInput;
        protected TextField infoInput;
        protected TextField priceInput;
        protected TextField availabilityInput;
        private User currentUser;
        public bool status;
        public bool exit;
        public ViewProduct()
        {
            this.Title = "Preview of product";
            
            Label id = new Label (3, 6, "Id:");
            Label name = new Label (3, 8, "Name:");
            Label info = new Label (3, 10, "Information:");
            Label price = new Label (3, 12, "Price(UA):");
            Label availability = new Label (3, 14, "Avalability:");
            
            this.Add(id, name, info, price, availability);

            idInput = new TextField (){X = 17, Y = Pos.Top(id), Width = 5, Text = "", ReadOnly = true};
            nameInput = new TextField (){X = 17, Y = Pos.Top(name), Width = 25, Text = "", ReadOnly = true};
            infoInput = new TextField (){X = 17, Y = Pos.Top(info), Width = 40, Text = "", ReadOnly = true};
            priceInput = new TextField (){X = 17, Y = Pos.Top(price), Width = 5, Text = "", ReadOnly = true};
            availabilityInput = new TextField(){X = 17, Y = Pos.Top(availability), Width = 5, Text = "", ReadOnly = true};

            Button viewAllInfo = new Button()
            {
                X = Pos.Right(infoInput) + 2,
                Y = Pos.Top(info),
                Text = "View all info"
            };
            viewAllInfo.Clicked += OnClickInfo;

            this.Add(idInput, nameInput, infoInput, priceInput, availabilityInput, viewAllInfo);

            Button modify = new Button()
            {
                X = 5,
                Y = Pos.Bottom(availabilityInput) + 5,
                Text = "Edit"
            };
            modify.Clicked += WindowModifAct;
            Button delete = new Button()
            {
                X = Pos.Right(modify) + 2,
                Y = Pos.Bottom(availabilityInput) + 5,
                Text = "Delete"
            };
            delete.Clicked += OnActDelete;
            Button exit = new Button()
            {
                X = Pos.Right(delete) + 2,
                Y = Pos.Bottom(availabilityInput) + 5,
                Text = "Exit"
            };
            exit.Clicked += WindowBackToMain;
            Button supplementOrder = new Button()
            {
                X = 5,
                Y = Pos.Bottom(modify) + 2,
                Text = "Supplement order"
            };
            supplementOrder.Clicked += OnClickedSupplementOrder;
            Button newOrder = new Button()
            {
                X = Pos.Right(supplementOrder) + 2,
                Y = Pos.Bottom(modify) + 2,
                Text = "New order"
            };
            newOrder.Clicked += OnClickedAddingOrder;
            this.Add(modify, delete, exit, supplementOrder, newOrder);

        }
        private void OnClickInfo()
        {
            MessageBox.Query("Information about product", infoInput.Text ,"Ok");
        }
        public Product GetProduct()
        {
            return this.product;
        }
        public void SetProduct(Product pro)
        {
            this.product = pro;
            this.idInput.Text = pro.id.ToString();
            this.nameInput.Text = pro.name;
            this.infoInput.Text = pro.info;
            this.priceInput.Text = pro.price.ToString();
            this.availabilityInput.Text = pro.availability.ToString();
        }
        public void SetUser(User user)
        {
            this.currentUser = user;
        }

        public void SetRepos(OrderRepository repoOr, CheckRepository repoCh, ProductRepository prods)
        {
            this.checkRepository = repoCh;
            this.orderRepository = repoOr;
            this.productRepository = prods;
        }

        private void WindowBackToMain()
        {
            this.exit = true;
            Application.RequestStop();
        }

        private void WindowModifAct()
        {
            if(currentUser.status == "moderator")
            {
                ModifyProduct window = new ModifyProduct()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                window.SetProduct(this.product);
                Application.Run(window);
                if(window.status)
                {
                    Product changed = window.GetProduct();
                    this.edited = true;
                    this.SetProduct(changed);
                }
            }
            else
            {
                MessageBox.ErrorQuery("Edit product","Only moderator can edit products", "OK");
            }
        }
        private void OnActDelete()
        {
            if(currentUser.status == "moderator")
            {
                int index = MessageBox.Query("Delete", "Are u sure?", "yes", "no");
                if(index == 0)
                {
                    this.del = true;
                    this.exit = true;
                    Application.RequestStop();
                }
            }
            else
            {
                MessageBox.ErrorQuery("Delete product","Only moderator can delete products", "OK");
            }
        }
        
        public void OnClickedAddingOrder()
        {
            if(!this.product.availability)
            {
                MessageBox.Query("To Order","This product is out of stock, please select another", "OK");
            }
            else
            {
                CreateOrder windowAdd = new CreateOrder()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                windowAdd.SetProduct(GetProduct());
                windowAdd.SetUser(currentUser);
                Application.Run(windowAdd);

                if(windowAdd.status)
                {
                   
                    Order newOrder = windowAdd.GetOrder();
                    long ordId = orderRepository.Insert(newOrder, System.DateTime.Now);
                    newOrder.id = ordId;
                    Check newCheck = new Check()
                    {
                        productId = this.product.id,
                        orderId = ordId,
                    };
                    long checkId = checkRepository.Insert(newCheck); 
                
                    ViewOrder viewWindow = new ViewOrder();
                    viewWindow.SetRepositories(productRepository, orderRepository);
                    viewWindow.SetOrder(orderRepository.GetById(newOrder.id));
                    Application.Run(viewWindow); 
                }
            }       
        }
        public void OnClickedSupplementOrder()
        {
            if(orderRepository.GetByUserId(this.currentUser.id).Count == 0)
            {
                MessageBox.Query("Supplement Order","You have no orders yet, please create it", "OK");
                return;
            }
            else
            {
                if(!this.product.availability)
                {
                    MessageBox.Query("To Order","This product is out of stock, please select another", "OK");
                }
                else
                {
                    CreateOrder windowAdd = new CreateOrder()
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill(),
                        Height = Dim.Fill()
                    };
                    windowAdd.SetProduct(GetProduct());
                    windowAdd.SetUser(currentUser);
                    Application.Run(windowAdd);

                    if(windowAdd.status)
                    {
                        Order lastOrder = orderRepository.GetByUserId(this.currentUser.id).Last();
                        Check newCheck = new Check()
                        {
                            productId = this.product.id,
                            orderId = lastOrder.id,
                        };
                        checkRepository.Insert(newCheck);

                        ViewOrder viewWindow = new ViewOrder();
                        viewWindow.SetRepositories(productRepository, orderRepository);
                        viewWindow.SetOrder(orderRepository.GetById(lastOrder.id));
                        Application.Run(viewWindow); 
                    }
                }
            }
        }
        public void checkClickProduct(ViewOrder viewWindow, long ordId)
        {   
            if(viewWindow.del)
            {
                int resCheck = checkRepository.DeleteByOrderId(ordId);
                int res = orderRepository.DeleteById(ordId);
                if(res != 1)
                {
                    MessageBox.ErrorQuery("Delete product","Product cannot be deleted", "OK");
                }
            }
            if(viewWindow.edited)
            {
                int res = orderRepository.UpdateById(ordId, viewWindow.GetOrder());
                if(res != 1)
                {
                    MessageBox.ErrorQuery("Edit product","Product cannot be edited", "OK");
                }
            }
            
        }
    }
}