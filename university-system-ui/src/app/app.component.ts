import { Component } from '@angular/core';
import { UserModel } from './models/userModel';
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'university-system-ui';
  date: Date;
  user: UserModel | null

  constructor(private authService: AuthService) {
    this.date = new Date();
    this.user = this.authService.parseJwt();
  }

  signedIn(): boolean {
    this.user = this.authService.parseJwt();
    return this.authService.isSignedIn();
  }

  isInRole(role: string): boolean {
    return this.authService.isInRole(role);
  }

  signOut(): void {
    this.authService.logout();
  }
}
