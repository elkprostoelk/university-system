import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { UserModel } from './models/userModel';
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnChanges {
  title = 'university-system-ui';
  date: Date;
  user: UserModel | null

  constructor(private authService: AuthService) {
    this.date = new Date();
    this.user = this.authService.parseJwt();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.user = this.authService.parseJwt();
  }

  signedIn(): boolean {
    return this.authService.isSignedIn();
  }

  signOut(): void {
    this.authService.logout();
  }
}
