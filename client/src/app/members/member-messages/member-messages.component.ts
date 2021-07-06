import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.scss']
})
export class MemberMessagesComponent implements OnInit {
  @Input() username:string;
  @Input() messages:Message[];
  content:string;
  constructor(private messageService:MessageService) { }

  ngOnInit(): void {
  }

  sendMessage(){
    this.messageService.sendMessage(this.username,this.content).subscribe(message=>{
      this.messages.push(message);
    })
  }





}
