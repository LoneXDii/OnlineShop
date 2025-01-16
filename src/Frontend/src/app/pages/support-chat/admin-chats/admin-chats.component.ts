import {Component, inject, OnInit} from '@angular/core';
import {SignalRService} from '../../../data/services/signal-r.service';
import {Chat} from '../../../data/interfaces/signalR/chat.interface';
import {ChatsListComponent} from '../components/chats-list/chats-list.component';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-admin-chats',
  imports: [
    ChatsListComponent,
    NgIf
  ],
  templateUrl: './admin-chats.component.html',
  styleUrl: './admin-chats.component.css'
})
export class AdminChatsComponent implements OnInit {
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

  configureSignalRService() {
    this.signalRService.receiveAllChats(chats => {
      this.chats = chats;
    });

    const connection = this.signalRService.connect();

    if (connection) {
      connection.then(() => {
        this.signalRService.getAllChats();
      });
    }
    else{
      this.signalRService.getAllChats();
    }
  }
}
