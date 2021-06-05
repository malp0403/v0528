import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<any> {
  canDeactivate(component: MemberEditComponent):boolean{
    if(component.editForm.dirty){
      return confirm("unsave changes will be lost")
    }
    return true;
  }
  
}
