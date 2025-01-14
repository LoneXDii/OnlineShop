import {inject, Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {Chat} from '../interfaces/signalR/chat.interface';
import {ChatMessage} from '../interfaces/signalR/chatMessage.interface';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  authService = inject(AuthService);
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/chat`, {
        withCredentials: true,
        accessTokenFactory: () => this.authService.getAccessToken
      })
      .withAutomaticReconnect()
      .build();
  }

  connect(){
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  getAllChats(){
    return this.hubConnection.invoke('GetAllChatsAsync');
  }

  receiveAllChats(){
    return new Observable<Chat[]>((observer) => {
      this.hubConnection.on("ReceiveChats", (chats: Chat[]) => {
        observer.next(chats);
      });
    });
  }

  getUserChats(){
    return this.hubConnection.invoke('GetUserChatsAsync');
  }

  receiveUserChats(){
    return new Observable<Chat[]>((observer) => {
      this.hubConnection.on("ReceiveUserChats", (chats: Chat[]) => {
        observer.next(chats);
      });
    });
  }

  getChatMessages(){
    return this.hubConnection.invoke('GetChatMessagesAsync');
  }

  receiveChatMessages(){
    return new Observable<ChatMessage[]>((observer) => {
      this.hubConnection.on("ReceiveChatMessages", (messages: ChatMessage[]) => {
        observer.next(messages);
      });
    });
  }

  createChat(){
    return this.hubConnection.invoke('CreateChatAsync');
  }

  receiveNewChat(){
    return new Observable<Chat>((observer) => {
      this.hubConnection.on("ReceiveNewChat", (chat: Chat) => {
        observer.next(chat);
      });
    });
  }

  closeChat(chatId: number){
    return this.hubConnection.invoke('CloseChatAsync', chatId);
  }

  receiveClosedChat(){
    return new Observable<number>((observer) => {
      this.hubConnection.on("ReceiveNewChat", (chatId: number) => {
        observer.next(chatId);
      });
    });
  }

  sendMessage(message: {text: string, chatId: number}) {
    return this.hubConnection.invoke('SendMessageAsync', message);
  }

  receiveMessage(){
    return new Observable<ChatMessage>((observer) => {
      this.hubConnection.on("ReceiveMessage", (message: ChatMessage) => {
        observer.next(message);
      });
    });
  }
}
