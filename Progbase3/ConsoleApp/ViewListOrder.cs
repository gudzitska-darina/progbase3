using Terminal.Gui;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class ViewListOrder : Window
    {
        private int pageLength = 10;
        private int numPage = 1;
        private Label page;
        private Label totalPage;
        private ListView allOrdListView;
        private OrderRepository orderRepository;
        private ProductRepository productRepository;
        private CheckRepository checkRepository;
        public ViewListOrder()
        {
            this.Title = "List of orders: ";
            
            Label textP = new Label("Total page:"){X = 2, Y = 4, Width = 15};
            totalPage = new Label("?")
            {
                X= Pos.Right(textP) + 2,
                Y = 4,
                Width = 5
            };
            allOrdListView = new ListView(new List<Order>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            allOrdListView.OpenSelectedItem += OnOpenOrder;
            FrameView frameView = new FrameView("Order:")
            {
                X = 2,
                Y = 8,
                Width = Dim.Fill() - 4,
                Height = pageLength + 1
            };
            frameView.Add(allOrdListView);
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

            this.Add(textP,totalPage, frameView);
            this.Add(prev, page, next);
            Button back = new Button(){X = 3, Y = Pos.Bottom(prev) + 3, Text = "Back to main window"};
            back.Clicked += WindowBackToMain;
            this.Add(back);
        }
        public ListView GetOrdList()
        {
            return this.allOrdListView;
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
            int totalP = orderRepository.GetTotalPages(pageLength);
            if(numPage >= totalP)
            {   
                return;
            }
            this.numPage += 1;
            ShowCurrentPage();

        }
        private void OnOpenOrder(ListViewItemEventArgs args)
        {
            ViewOrder viewWindOrd = new ViewOrder();
            viewWindOrd.SetRepositories(productRepository, orderRepository);

            Order ord = (Order)args.Value;
            viewWindOrd.SetOrder(ord);
            Application.Run(viewWindOrd);
            
            if(viewWindOrd.exit)
            {
                checkClickOrder(viewWindOrd, ord.id);
            }
            
        }
        public void SetOrderRepository(OrderRepository repo, CheckRepository checks, ProductRepository prods)
        {
            this.orderRepository  = repo;
            this.checkRepository = checks;
            this.productRepository = prods;
            this.ShowCurrentPage();
        }
        
        private void ShowCurrentPage()
        {
            this.page.Text = numPage.ToString();
            this.totalPage.Text = orderRepository.GetTotalPages(pageLength).ToString();
            this.allOrdListView.SetSource(orderRepository.GetPage(numPage, pageLength));
        }

        public void checkClickOrder(ViewOrder viewWindow, long ordId)
        {   
            if(viewWindow.del)
            {
                int checkRes = checkRepository.DeleteByOrderId(ordId);
                int res = orderRepository.DeleteById(ordId);
                if(res == 1)
                {
                    int pages = orderRepository.GetTotalPages(pageLength);
                    if(numPage > pages)
                    {
                        numPage -= 1;
                    }
                    if(numPage == 0)
                    {
                        MessageBox.ErrorQuery("List of orders is empty", "OK");
                    }
                    allOrdListView.SetSource(orderRepository.GetPage(numPage, pageLength));
                }
                else
                {
                    MessageBox.ErrorQuery("Delete order","Order cannot be deleted", "OK");
                }
            }
            if(viewWindow.edited)
            {
                int res = orderRepository.UpdateById(ordId, viewWindow.GetOrder());
                if(res == 1)
                {
                    allOrdListView.SetSource(orderRepository.GetPage(numPage, pageLength));
                }
                else
                {
                    MessageBox.ErrorQuery("Edit order","Order cannot be edited", "OK");
                }
            }
        }
        
    }
}