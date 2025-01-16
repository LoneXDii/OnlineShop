import {Component, inject, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {Chat} from '../../../../data/interfaces/signalR/chat.interface';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../../../data/services/auth.service';
import {SignalRService} from '../../../../data/services/signal-r.service';

@Component({
  selector: 'app-chats-list',
  imports: [
    RouterLink
  ],
  templateUrl: './chats-list.component.html',
  styleUrl: './chats-list.component.css'
})
export class ChatsListComponent implements OnChanges, OnInit {
  @Input() chats!: Chat[];
  signalRService = inject(SignalRService);
  authService = inject(AuthService);
  activeChats: Chat[] = [];
  closedChats: Chat[] = [];

  ngOnInit() {
    this.configureSignalRService();
  }

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

  configureSignalRService(){
    this.signalRService.receiveNewChat(chat => {
      this.chats.push(chat);
      this.groupChats(this.chats);
    });

    this.signalRService.receiveClosedChat(chatId => {
      this.onChatClosed(chatId);
      this.groupChats(this.chats);
    });

    this.signalRService.connect();
  }

  onChatClosed(chatId: number){
    const chat = this.chats.find(c => c.id === chatId);

    if (chat) {
      chat.isActive = false;
    }
  }
}
