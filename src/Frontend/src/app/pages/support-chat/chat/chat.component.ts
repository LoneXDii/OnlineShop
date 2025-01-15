import {AfterViewInit, Component, ElementRef, inject, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {SignalRService} from '../../../data/services/signal-r.service';
import {ActivatedRoute} from '@angular/router';
import {Chat} from '../../../data/interfaces/signalR/chat.interface';
import {AuthService} from '../../../data/services/auth.service';
import {ChatMessage} from '../../../data/interfaces/signalR/chatMessage.interface';
import {DatePipe, NgIf} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-chat',
  imports: [
    NgIf,
    FormsModule,
    DatePipe
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
  userId: string;

  constructor() {
    this.userId = this.authService.getUserId;
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
    this.signalRService.receiveChatMessages(messages => {
      this.messages = messages.map((message: ChatMessage) => ({
        ...message,
        dateTime: message.dateTime + 'Z'
      }));
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

    const connection = this.signalRService.connect();

    if (connection) {
      connection.then(() => {
        this.getData();
      });
    }
    else{
      this.getData();
    }
  }

  getData(){
    this.route.params.subscribe(
      params => {
        const chatId = +params['id'];
        this.signalRService.getChatById(chatId)
          .catch(error => console.log(error));
        this.signalRService.getChatMessages(chatId)
          .catch(error => console.log(error));
      });
  }
}
