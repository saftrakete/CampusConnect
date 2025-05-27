import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountSettingsComponent } from './account-settings/account-settings.component';
import { EmailSettingsComponent } from './email-settings/email-settings.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminGuard } from './routeguards/AdminGuard';
import { AuthGuardTestComponent } from './auth-guard-test/auth-guard-test.component';
import { AuthGuard } from './routeguards/AuthGuard';
import { ForbiddenComponent } from './forbidden/forbidden.component';

export const baseApiRoute = 'https://localhost:7109/';

const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: '',
        component: HomeComponent,
        pathMatch: 'full'
    },
    {
        path: 'accountsettings',
        component: AccountSettingsComponent
    },
    {
        path: 'emailsettings',
        component: EmailSettingsComponent
    },
    {
        path: "adminpanel",
        component: AdminPanelComponent,
        canActivate: [AdminGuard]
    },
    {
        path: "authguardtest",
        component: AuthGuardTestComponent,
        canActivate: [AuthGuard]
    },
    {
        path: "forbidden",
        component: ForbiddenComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
