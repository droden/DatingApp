import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingApp-SPA';
  jtwHelper = new JwtHelperService();

  /**
   *
   */
  constructor(private authService: AuthService) {
  }

   /**
   * try and load token from local storage when app starts so page refresh dont misbehave
   */
  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jtwHelper.decodeToken(token);
    }
  }
}
