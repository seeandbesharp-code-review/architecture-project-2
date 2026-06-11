namespace SaleApi.Dto
{
    public class CategoryDto
    {
        public class GetCategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }

        }

        public class CreateCategoryDto
        {
            public string Name { get; set; }
            public string Color { get; set; }
        }

        public class DeleteCategoryDto
        {
            public int Id { get; set; }
        }
    }
}
