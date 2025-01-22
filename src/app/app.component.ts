import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(private activatedRoute: ActivatedRoute) {}
  title = 'my-dashboard';
}
