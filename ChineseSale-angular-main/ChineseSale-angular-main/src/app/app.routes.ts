import { Routes } from '@angular/router';
import { Home} from './component/home/home';
import { DonorComponent } from './component/Donor/DonorComponent/DonorComponent';
import { AddDonor } from './component/Donor/add-donor/add-donor';
import { UpdateDonor } from './component/Donor/update-donor/update-donor';
import { ListGift } from './component/Gift/list-gift/list-gift';
import { Register } from './component/Auth/register/register';
import { Login } from './component/Auth/login/login';
import { Baglist } from './component/Bag/bag-list/bag-list';
import { CategoryList } from './component/Category/category-list/category-list';
import { OrderList } from './component/Order/order-list/order-list';
import { Random } from './component/random/random';
import { AllWinners } from './component/all-winners/all-winners';
import { OrderHistory } from './component/Order/order-history/order-history';

export const routes: Routes = [
    { path: '', component: Home },
    {path:'donor',component:DonorComponent},
    {path:'add-donor',component:AddDonor},
    {path:'update-donor',component:UpdateDonor},
    {path:'gift',component:ListGift},
    {path:'register',component:Register},
    {path:'login',component:Login},
    {path:'bag',component:Baglist},
    {path:'category',component:CategoryList},
    {path:'order',component:OrderList},
    {path:'random',component:Random},
    {path:'winner',component:AllWinners},
    {path:'order-history',component:OrderHistory},



];