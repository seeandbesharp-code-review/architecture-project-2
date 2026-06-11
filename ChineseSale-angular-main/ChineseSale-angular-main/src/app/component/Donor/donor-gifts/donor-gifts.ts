// import { Component, Input, signal } from '@angular/core';
// import { GiftService } from '../../../service/gift.service';
// import { Gift } from '../../../models/gift.model';
// import { DonorService } from '../../../service/donor.service';

// @Component({
//   selector: 'app-donor-gifts',
//   imports: [],
//   templateUrl: './donor-gifts.html',
//   styleUrl: './donor-gifts.scss',
// })
// export class DonorGifts {
// @Input() donorId!: number;
//   gifts = signal<Gift[]>([]);

//   constructor(private donorService: DonorService) {}

//   ngOnInit() {
//     // שליפה מה-API רק כשצריך
// // this.donorService.donorGifts(this.donorId).subscribe((data: any[]) => {
// //     this.gifts.set(data);
//     // });
//   }
// }
