namespace ConsoleApp
{
    public class ModifyProduct: CreateProduct
    {
        public ModifyProduct()
        {
            this.Title = "Edit product";
        }
        public void SetProduct(Product pro)
        {
            this.nameInput.Text = pro.name;
            this.infoInput.Text = pro.info;
            this.priceInput.Text = pro.price.ToString();
            this.availabilityInput.Text = pro.availability.ToString();
        } 
    }  
}