
export interface Gift {
    id: number;
   name: string;
    description: string;
    img: string;
    price: number;
    idDoner: number;
    categoryId?: number;
}

export interface AddGiftDto {
   name: string;
    description: string;
    image?: File;
    price: number;
    idDoner: number;
    categoryId?: number;
}

export interface UpdateGiftDto {
   name: string;
    description: string;
    img: string;
    price: number;
    idDoner: number;
    categoryId?: number;
}

//   public class UpdateGiftDto
//   {
//       [Required]
//       public int Id { get; set; }
      
//       [MaxLength(100)]
//       public string? Name { get; set; }
//       [MaxLength(500)]
//       public string? Description { get; set; }

//       [MaxLength(1000)]
//       public string? Img { get; set; }

//       public int Price { get; set; }
//        public int IdDoner { get; set; }
//        public int? CategoryId { get; set; }
//   }