using Terminal.Gui;
namespace ConsoleApp
{
    public class AunthenticationWindow : Window
    {
        protected TextField loginInput;
        protected TextField passwordInput;
        public User logInUser;
        private Authentication userAuthentication;
        private UserRepository userRepository;
        private ProductRepository productRepository;
        private OrderRepository orderRepository;
        private CheckRepository checkRepository;
        public AunthenticationWindow()
        {
            MenuBar menu = new MenuBar(new MenuBarItem[] {
                    new MenuBarItem ("_File", new MenuItem [] {
                        new MenuItem ("_Quit", "", OnQuit)
                    }),
                });
            this.Add(menu);

            Label login = new Label ("Login:  "){X = Pos.Percent(33), Y = Pos.Percent(40)};
            Label password = new Label ("Password:"){X = Pos.Percent(33), Y = Pos.Bottom(login) + 2};   
            this.Add(login, password);

            this.loginInput = new TextField (){X = Pos.Right(login) + 3, Y = Pos.Percent(40), Width = 15, Text = ""};
            this.passwordInput = new TextField (){X = Pos.Right(password) + 2, Y = Pos.Bottom(loginInput) + 2, Width = 15, Text = "", Secret = true};
            this.Add(loginInput, passwordInput);

            Button logIn = new Button(){X = Pos.Percent(35), Y = Pos.Bottom(passwordInput) + 5, Text = "Log In"};
            logIn.Clicked += OnClickedLog;
            Button regesrt = new Button(){X = Pos.Right(logIn) + 6, Y  = Pos.Bottom(passwordInput) + 5, Text = "Register"};
            regesrt.Clicked += OnClickedReg;

            this.Add(logIn, regesrt);
        }

        public void SetUserRepositoryAndAunth(Authentication auth, UserRepository repo)
        {
            this.userAuthentication = auth;
            this.userRepository = repo;
        }
        
        public User GetUser()
        {
            return this.logInUser;
        }

        public void OnClickedLog()
        {
            long res = userAuthentication.Login(loginInput.Text.ToString(), passwordInput.Text.ToString());
            if(res == 0)
            {
                MessageBox.ErrorQuery("LogIn","User not found, please register", "OK");
            }
            else if(res == -1)
            {
                MessageBox.ErrorQuery("LogIn","Password is incorrect, please try again", "OK");
            }
            else
            {
                this.logInUser = userRepository.GetById(res);
                OpenMainWindow();
            }
        }
        public void OnClickedReg()
        {
            long res = userAuthentication.Registration(this.loginInput.Text.ToString(), this.passwordInput.Text.ToString());
            if(res == 0)
            {
                MessageBox.ErrorQuery("LogIn","User is already registered", "OK");
            }
            else
            {
                this.logInUser = userRepository.GetById(res);
                OpenMainWindow();
            }
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
        private void OpenMainWindow()
        {
            MainWindow win = new MainWindow();
            win.SetOrderRepository(orderRepository);
            win.SetProductRepository(productRepository);
            win.SetCheckRepository(checkRepository);
            win.SetUser(GetUser());

            Application.Run(win);
        }
        private void OnQuit()
        {
            Application.RequestStop();
        }  
    }
}