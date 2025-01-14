import {Component, inject, OnInit} from '@angular/core';
import {SignalRService} from '../../../data/services/signal-r.service';
import {Chat} from '../../../data/interfaces/signalR/chat.interface';
import {JsonPipe} from '@angular/common';

@Component({
  selector: 'app-admin-chats',
  imports: [
    JsonPipe
  ],
  templateUrl: './admin-chats.component.html',
  styleUrl: './admin-chats.component.css'
})
export class AdminChatsComponent implements OnInit {
  signalRService = inject(SignalRService);
  chat: Chat | null = null;

  ngOnInit(){
    this.signalRService.connect()
      .subscribe(() => {
        this.signalRService.receiveNewChat()
          .subscribe((chat) => {
            this.chat = chat;
          });
      });
  }

  createChat(){
    this.signalRService.createChat();
  }
}
