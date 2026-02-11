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
      style={{ width: "100%" }}
      dataSource={comments ?? []}
      itemLayout="vertical"
      split={false}
      rowKey={(comment) => comment.id}
      renderItem={(comment) => (
        <List.Item style={{ padding: "0 0 12px" }}>
          <CommentComponent comment={comment} />
        </List.Item>
      )}
      footer={<div style={{ height: 24 }} />} 
    />
  );
  


}

export default CommentsListComponent;
