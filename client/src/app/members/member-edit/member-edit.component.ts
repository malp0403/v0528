import { Component, HostListener, OnInit } from '@angular/core';
import { Form, FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit {
  member: Member;
  user: User;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  editForm: FormGroup;

  constructor(private accountService: AccountService,
    private membersService: MembersService,
    private fb: FormBuilder,
    private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    })
  }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      introduction: null,
      lookingFor: null,
      interests: null,
      city: null,
      country: null
    })
    this.loadMember();


  }

  loadMember() {
    const username = this.user.username;
    this.membersService.getMember(username).subscribe(member => {
      this.member = member;
      this.editForm.patchValue({ ...this.member });
      console.log('member: ', this.member);
    }

    )
  }

  updateMember() {
    const member = this.editForm.getRawValue();
    this.membersService.updateMember(member).subscribe(() => {
      this.toastr.success('Profile updated successfully!');
      this.editForm.markAsPristine();
    })

  }

}
