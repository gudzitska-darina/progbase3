using System;
using Terminal.Gui;

namespace ConsoleApp
{
    public class UserInfoDialog : Dialog
    {
        private User currentUser;
        private TextField idInput;
        private TextField loginInput;
        private TextField passwordInput;
        private TextField statusInput;
        private TextField numOrdersInput;
        public UserInfoDialog()
        {
            this.Title = "Info about user";

            Label id = new Label (3, 6, "Id:");
            Label login = new Label (3, 8, "Login:");
            Label pasword = new Label(3, 10, "Password:");
            Label status = new Label (3, 12, "Status:");
            Label orders = new Label (3, 14, "The number of orders:");
            this.Add(id, login, pasword, status, orders);

            idInput = new TextField (){X = 25, Y = Pos.Top(id), Width = 5, Text = "", ReadOnly = true};
            loginInput = new TextField(){X = 25, Y = Pos.Top(login), Width = 20, Text = "", ReadOnly = true};
            passwordInput = new TextField (){X = 25, Y = Pos.Top(pasword), Width = 25, Text = "", ReadOnly = true};
            statusInput = new TextField (){X = 25, Y = Pos.Top(status), Width = 15, Text = "", ReadOnly = true};
            numOrdersInput = new TextField (){X = 25, Y = Pos.Top(orders), Width = 3, Text = "", ReadOnly = true};
            this.Add(idInput, loginInput, passwordInput, statusInput, numOrdersInput);

            Button exit = new Button(){X = Pos.Percent(50), Y = Pos.Bottom(orders) + 3, Text = "Back"};
            exit.Clicked += WindowBackToMain;
            this.Add(exit);
        }

        public void SetUser(User user)
        {
            this.currentUser = user;
            this.idInput.Text = user.id.ToString();
            this.loginInput.Text = user.login.ToString();
            this.passwordInput.Text = user.password.ToString();
            this.statusInput.Text = user.status.ToString();
            this.numOrdersInput.Text = user.orders.Count.ToString();
        }
        private void WindowBackToMain()
        {
            Application.RequestStop();
        }
    }
}