import {Component, inject, OnInit} from '@angular/core';
import {SignalRService} from '../../../data/services/signal-r.service';
import {Chat} from '../../../data/interfaces/signalR/chat.interface';
import {ChatsListComponent} from '../components/chats-list/chats-list.component';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-user-chats',
  imports: [
    ChatsListComponent,
    NgIf
  ],
  templateUrl: './user-chats.component.html',
  styleUrl: './user-chats.component.css'
})
export class UserChatsComponent implements OnInit {
  signalRService = inject(SignalRService);
  chats: Chat[] = [];

  ngOnInit(){
    this.configureSignalRService();
  }

  onChatClosed(chatId: number){
    const chat = this.chats.find(c => c.id === chatId);

    if (chat) {
      chat.isActive = false;
    }
  }

  createChat(){
    this.signalRService.createChat()
      .catch(err => alert("You already have an opened chat"));
  }

  configureSignalRService(){
    this.signalRService.receiveUserChats(chats => {
      this.chats = chats;
    });

    const connection = this.signalRService.connect();

    if (connection) {
      connection.then(() => {
        this.signalRService.getUserChats();
      });
    }
    else{
      this.signalRService.getUserChats();
    }
  }
}
