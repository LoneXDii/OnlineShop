import {Component, inject, OnInit} from '@angular/core';
import {SignalRService} from '../../../data/services/signal-r.service';
import {ActivatedRoute} from '@angular/router';
import {Chat} from '../../../data/interfaces/signalR/chat.interface';
import {AuthService} from '../../../data/services/auth.service';
import {ChatMessage} from '../../../data/interfaces/signalR/chatMessage.interface';
import {NgIf} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-chat',
  imports: [
    NgIf,
    FormsModule
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit {
  signalRService = inject(SignalRService);
  authService = inject(AuthService);
  route = inject(ActivatedRoute);
  chat: Chat | null = null;
  messages: ChatMessage[] = [];
  currentInputText: string = '';

  //temporary
  chatId: number | null = null;

  constructor() {
    this.route.params.subscribe(
      params => {
        this.chatId = +params['id'];
      });
  }


  ngOnInit() {
    this.configureSignalRService();
  }

  onSendMessage(){
    if(this.currentInputText !== '' && this.chat) {
      this.signalRService.sendMessage({text: this.currentInputText, chatId: this.chat.id})
      this.currentInputText = '';
    }
  }

  private configureSignalRService() {
    this.signalRService.connect();

    this.signalRService.receiveChatMessages(messages => {
      this.messages = messages;
    });

    this.signalRService.receiveChat(chat => {
      this.chat = chat
    });

    this.signalRService.receiveMessage(message => {
      if (this.chat?.clientId === message.chatOwnerId) {
        this.messages.push(message)
      }
    });

    this.signalRService.receiveClosedChat(chatId => {
      if(this.chat?.id === chatId){
        this.chat.isActive = false;
      }
    });

    //TODO
    //Fix this (not working)
    this.route.params.subscribe(
      params => {
        const chatId = +params['id'];
        this.signalRService.getChatById(chatId)
          .catch(error => console.log(error));
        this.signalRService.getChatMessages(chatId)
          .catch(error => console.log(error));
      })
  }
}
