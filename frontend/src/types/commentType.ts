import type { UserProfile } from "./auth";

export interface CreateCommentsRequest {
  content: string;
}

export interface UpdateCommentRequest {
  content: string;
}

export interface CommentResponse {
  id: number;
  content: string;
  user: UserProfile;
  createdAt: string;
  updatedAt: string;
}