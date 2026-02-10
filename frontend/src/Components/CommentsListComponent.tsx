import { List } from "antd";
import { useGetAllCommentsByMovieId } from "../Queries/Comments/Comments";
import CommentComponent from "./CommentComponent";

type Props = {
  movieId: number;
}

function CommentsListComponent({ movieId }: Props) {

  const { data: comments, isLoading, error } = useGetAllCommentsByMovieId(movieId);

  if (isLoading) return <div>Loading....</div>;
  if (error) return <div>Something went wrong</div>;


  return (
    <List
      style={{ maxHeight: 320, overflowY: "auto", width: "100%", paddingInline: 0 }}
      dataSource={comments ?? []}
      itemLayout="vertical"
      split={false}
      rowKey={(comment) => comment.id}
      renderItem={(comment) => (
        <List.Item style={{ padding: "0 0 12px" }}>
          <CommentComponent comment={comment} />
        </List.Item>
      )}
    />
  );
  


}

export default CommentsListComponent;
