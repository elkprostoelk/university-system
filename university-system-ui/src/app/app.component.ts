import { Component } from '@angular/core';
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'university-system-ui';
  date: Date;
  

  constructor(private authService: AuthService) {
    this.date = new Date();
    
  }

  signedIn(): boolean {
    return this.authService.isSignedIn();
  }

  signOut(): void {
    this.authService.logout();
  }
}
