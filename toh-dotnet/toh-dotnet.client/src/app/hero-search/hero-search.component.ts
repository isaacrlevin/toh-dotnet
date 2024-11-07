import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { HeroService } from '../hero.service';
import { ReactiveFormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-hero-search',
  imports: [
    NgFor,
    NgIf,
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './hero-search.component.html',
  styleUrl: './hero-search.component.css'
})
export class HeroSearchComponent implements OnInit {
  searchControl = new FormControl('');
  heroes: any[] = [];

  constructor(private searchService: HeroService) { }

  ngOnInit() {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),  // Wait for 300ms pause in typing
      distinctUntilChanged(),  // Only emit if value is different from before
      switchMap(query => {
        // Make API call only if there is a non-empty query
        return query ? this.searchService.searchHeroes(query) : of([]);
      }),
      catchError(error => {
        console.error('Search error:', error);
        return of([]);  // Return an empty array on error
      })
    ).subscribe(data => this.heroes = data);
  }
}
