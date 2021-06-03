namespace ConsoleApp
{
    public class ModifyOrder: CreateOrder
    {
        public ModifyOrder()
        {
            this.Title = "Modificate Order";
        }
        public void SetOrder(Order ord)
        {
            this.isPayedInput.Checked = ord.isPayed;
            this.currentUser = ord.autor;
        } 
    }  
}