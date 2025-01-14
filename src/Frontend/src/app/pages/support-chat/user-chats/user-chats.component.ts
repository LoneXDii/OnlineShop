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
    this.signalRService.createChat();
  }

  private configureSignalRService(){
    this.signalRService.connect();

    this.signalRService.receiveUserChats(chats => {
      this.chats = chats;
    });

    this.signalRService.receiveNewChat(chat => {
      this.chats = [...this.chats, chat];
    });

    this.signalRService.receiveClosedChat(chatId => {
      this.onChatClosed(chatId);
    });

    //TODO
    //Fix this (not working)
    this.signalRService.getUserChats();
  }
}
