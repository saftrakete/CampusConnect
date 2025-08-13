import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import {MatDialogModule} from '@angular/material/dialog';
import {MatTooltipModule} from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { DeleteAccountComponent } from './delete-account/delete-account.component';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { AccountSettingsComponent } from './account-settings/account-settings.component';
import { SettingsSidebarComponent } from './settings-sidebar/settings-sidebar.component';
import { EmailSettingsComponent } from './email-settings/email-settings.component';
import { EditUsernameFormComponent } from './edit-username-form/edit-username-form.component';
import { JwtInterceptor } from './interceptors/JwtInterceptor';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AuthGuardTestComponent } from './auth-guard-test/auth-guard-test.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { NotFoundComponent } from './not-found/not-found.component';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    DeleteAccountComponent,
    UserSettingsComponent,
    AccountSettingsComponent,
    SettingsSidebarComponent,
    EmailSettingsComponent,
    EditUsernameFormComponent,
    ChatComponent,
    AdminPanelComponent,
    AuthGuardTestComponent,
    ForbiddenComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatTooltipModule,
    MatDividerModule,
    MatCardModule,
    MatListModule
  ],
  providers: [
    provideAnimationsAsync(),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
