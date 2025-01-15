import {Component, inject, Input, OnChanges, SimpleChanges} from '@angular/core';
import {Chat} from '../../../../data/interfaces/signalR/chat.interface';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../../../data/services/auth.service';

@Component({
  selector: 'app-chats-list',
  imports: [
    RouterLink
  ],
  templateUrl: './chats-list.component.html',
  styleUrl: './chats-list.component.css'
})
export class ChatsListComponent implements OnChanges {
  @Input() chats!: Chat[];
  authService = inject(AuthService);
  activeChats: Chat[] = [];
  closedChats: Chat[] = [];

  ngOnChanges(changes: SimpleChanges) {
    if(changes['chats']){
      this.groupChats(this.chats);
    }
  }

  groupChats(chats: Chat[]){
    this.activeChats = [];
    this.closedChats = [];

    for (let chat of chats){
      if(chat.isActive){
        this.activeChats.push(chat);
      }
      else{
        this.closedChats.push(chat);
      }
    }
  }
}
