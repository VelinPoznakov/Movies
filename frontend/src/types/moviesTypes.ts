import type { UserProfile } from "./auth";
import type { CommentResponse } from "./commentType";
import type { DirectorResponse } from "./directorsTypes";
import type { GenreResponse } from "./genreTypes";

export interface CreateMovieRequest {
  title: string;
  timeLong: string;
  directorName: string;
  genreName: string;
  releaseDate: string;
}

export interface UpdateMovieRequest {
  title: string;
  timeLong: string;
  directorName: string;
  genreName: string;
  releaseDate: string;
}

export interface MovieResponse {
  id: number;
  title: string;
  timeLong: string;
  director: DirectorResponse;
  releaseDate: string;
  genre: GenreResponse;
  rating: number;
  createdAt: string;
  updatedAt: string;
  user: UserProfile;
  comments: CommentResponse[];
}